using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnimatLabs.WorkflowEngine.Core;
using AnimatLabs.WorkflowEngine.Core.EventsArgs;
using AnimatLabs.WorkflowEngine.Core.RetryStrategy;
using AnimatLabs.WorkflowEngine.Exceptions;

namespace AnimatLabs.WorkflowEngine
{
    /// <summary>
    /// Executes workflows with compensation, middleware, and throttling
    /// </summary>
    public sealed class WorkflowRunner : ICloneable
    {
        private readonly IEnumerable<IWorkflowStepMiddleware> middleware;
        private readonly WorkflowSettings settings;
        private readonly ILogger logger;
        private readonly IRetryStrategy compensationRetryStrategy;
        private readonly ISystemClock systemClock;

        /// <inheritdoc />
        public event EventHandler<StepExecutingEventArgs> StepExecuting;

        /// <inheritdoc />
        public event EventHandler<StepExecutedEventArgs> StepExecuted;

        /// <inheritdoc />
        public event EventHandler<CompensationFailedEventArgs> CompensationFailed;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowRunner"/> class
        /// </summary>
        /// <param name="middleware">The middleware to be applied to the workflow steps.</param>
        /// <param name="settings">The settings for the workflow execution (optional).</param>
        /// <param name="logger">The logger for logging workflow execution details (optional).</param>
        /// <param name="compensationRetryStrategy">The retry strategy for compensations (optional).</param>
        /// <param name="systemClock">The system clock for getting the current time (optional).</param>
        public WorkflowRunner(
            IEnumerable<IWorkflowStepMiddleware> middleware,
            WorkflowSettings settings = null,
            ILogger logger = null,
            IRetryStrategy compensationRetryStrategy = null,
            ISystemClock systemClock = null
        )
        {
            this.middleware = new List<IWorkflowStepMiddleware>(middleware);
            this.settings = settings ?? new WorkflowSettings();
            this.logger = logger ?? new NullLogger();
            this.compensationRetryStrategy = compensationRetryStrategy ?? new FixedIntervalRetryStrategy(TimeSpan.FromMilliseconds(100), settings.CompensationRetries);
            this.systemClock = systemClock ?? new SystemClock();
        }

        /// <summary>
        /// Executes the workflow
        /// </summary>
        /// <param name="workflow">The workflow to be executed.</param>
        /// <param name="context">The workflow context containing shared data and services.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests (optional).</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="WorkflowException">Thrown when an error occurs during workflow execution.</exception>
        public async Task ExecuteAsync(IWorkflow workflow, IWorkflowContext context, CancellationToken cancellationToken = default)
        {
            int lastExecutedStepIndex = -1;
            using (_ = logger.BeginScope(new Dictionary<string, object>
            {
                ["CorrelationId"] = context.CorrelationId,
                ["WorkflowId"] = workflow.WorkflowId,
                ["WorkflowName"] = workflow.Name
            }))
            {
                try
                {
                    foreach (var step in workflow.Steps)
                    {
                        using (_ = logger.BeginScope(new Dictionary<string, object>
                        {
                            ["StepId"] = step.StepId,
                            ["StepName"] = step.Name
                        }))
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            StepExecuting?.Invoke(this, new StepExecutingEventArgs(step));
                            var startTime = systemClock.UtcNow;

                            await ExecuteStepWithMiddlewareAsync(step, context, cancellationToken).ConfigureAwait(false);

                            StepExecuted?.Invoke(this, new StepExecutedEventArgs(step, systemClock.UtcNow - startTime));
                            lastExecutedStepIndex++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError("Workflow failed!", ex);
                    if (settings.AutoCompensate)
                        await CompensateAsync(workflow, lastExecutedStepIndex, context, cancellationToken);
                    throw new WorkflowException("Workflow failed!", ex);
                }
            }
        }

        /// <summary>
        /// Executes a workflow step with middleware
        /// </summary>
        /// <param name="step">The workflow step to be executed.</param>
        /// <param name="context">The workflow context containing shared data and services.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task ExecuteStepWithMiddlewareAsync(
            IWorkflowStep step,
            IWorkflowContext context,
            CancellationToken cancellationToken
        )
        {
            Func<Task> pipeline = () => step.ExecuteAsync(context, cancellationToken);
            foreach (var middleware in middleware)
            {
                var next = pipeline;
                pipeline = () => middleware.ExecuteAsync(step, context, next, cancellationToken);
            }
            await pipeline();
        }

        /// <summary>
        /// Compensates the executed steps in case of failure
        /// </summary>
        /// <param name="workflow">The workflow instance.</param>
        /// <param name="lastExecutedStepIndex">The index of the last successfully executed step.</param>
        /// <param name="context">The workflow context containing shared data and services.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task CompensateAsync(
            IWorkflow workflow,
            int lastExecutedStepIndex,
            IWorkflowContext context,
            CancellationToken cancellationToken
        )
        {
            var compensationManager = context.WorkflowStepCompensationManager;
            var exceptions = new List<Exception>();

            for (int revertStepIndex = lastExecutedStepIndex; revertStepIndex >= 0; revertStepIndex--)
            {
                var step = workflow.Steps[revertStepIndex];
                using (_ = logger.BeginScope(new Dictionary<string, object> {
                    { "RetryStrategy", compensationRetryStrategy.GetType().Name }
                }))
                {
                    if (compensationManager.IsCompensated(workflow.WorkflowId, step.StepId))
                    {
                        continue;
                    }

                    try
                    {
                        await compensationRetryStrategy.ExecuteAsync(async () =>
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            await step.CompensateAsync(context, cancellationToken);
                            compensationManager.MarkCompensated(workflow.WorkflowId, step.StepId);
                        }, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                        CompensationFailed?.Invoke(this, new CompensationFailedEventArgs(step, ex));
                        if (!settings.ContinueOnCompensationFailure)
                        {
                            throw new WorkflowException("Compensation failed", new AggregateException(exceptions));
                        }
                    }
                }
            }

            if (exceptions.Count > 0)
            {
                throw new WorkflowException("One or more compensations failed", new AggregateException(exceptions));
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="WorkflowRunner"/> class that is a copy of the current instance.
        /// </summary>
        /// <returns>A new instance of the <see cref="WorkflowRunner"/> class.</returns>
        public object Clone()
        {
            return new WorkflowRunner(
                new List<IWorkflowStepMiddleware>(this.middleware),
                this.settings,
                this.logger,
                this.compensationRetryStrategy,
                this.systemClock
            );
        }
    }
}