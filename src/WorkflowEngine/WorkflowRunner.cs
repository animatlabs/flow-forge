using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WorkflowEngine.Core;
using WorkflowEngine.Core.EventsArgs;
using WorkflowEngine.Core.RetryStrategy;
using WorkflowEngine.Exceptions;

namespace WorkflowEngine
{
    /// <summary>
    /// Executes workflows with compensation, middleware, and throttling
    /// </summary>
    public sealed class WorkflowRunner
    {
        private readonly IEnumerable<IWorkflowStepMiddleware> middleware;
        private readonly WorkflowSettings settings;
        private readonly ILogger logger;
        private readonly IRetryStrategy compensationRetryStrategy;
        private readonly ISystemClock systemClock;
        private readonly SemaphoreSlim semaphore;

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
            this.middleware = middleware ?? Array.Empty<IWorkflowStepMiddleware>();
            this.settings = settings ?? new WorkflowSettings();
            this.logger = logger ?? new NullLogger();
            this.compensationRetryStrategy = compensationRetryStrategy ?? new FixedIntervalRetryStrategy(TimeSpan.FromMilliseconds(100), settings.CompensationRetries);
            this.systemClock = systemClock ?? new SystemClock();
            semaphore = new SemaphoreSlim(this.settings.MaxConcurrentWorkflows);
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
            await semaphore.WaitAsync(cancellationToken); // Acquire the semaphore
            try
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
                                var startTime = systemClock.UtcNow;

                                // Raise StepExecuting event
                                (workflow as Workflow).OnStepExecuting(new StepExecutingEventArgs(step));

                                await ExecuteStepWithMiddlewareAsync(step, context, cancellationToken).ConfigureAwait(false);

                                // Raise StepExecuted event
                                (workflow as Workflow).OnStepExecuted(new StepExecutedEventArgs(step, systemClock.UtcNow - startTime));

                                lastExecutedStepIndex++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError("Workflow failed!", ex);
                        if (settings.AutoCompensate)
                        {
                            try
                            {
                                await CompensateAsync(workflow, lastExecutedStepIndex, context, cancellationToken);
                            }
                            catch (Exception compensationEx)
                            {
                                // Raise CompensationFailed event
                                (workflow as Workflow).OnCompensationFailed(new CompensationFailedEventArgs(workflow.Steps[lastExecutedStepIndex], compensationEx));
                                throw;
                            }
                        }
                        throw new WorkflowException("Workflow failed!", ex);
                    }
                }
            }
            finally
            {
                semaphore.Release(); // Release the semaphore
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
                pipeline = async () =>
                {
                    logger.LogInformation($"Executing middleware: {middleware.GetType().Name} for step: {step.StepId}");
                    await middleware.ExecuteAsync(step, context, next, cancellationToken);
                    logger.LogInformation($"Executed middleware: {middleware.GetType().Name} for step: {step.StepId}");
                };
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
                            logger.LogInformation($"Compensating step: {step.StepId}");
                            await step.CompensateAsync(context, cancellationToken);
                            compensationManager.MarkCompensated(workflow.WorkflowId, step.StepId);
                            logger.LogInformation($"Compensated step: {step.StepId}");
                        }, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                        logger.LogError($"Compensation failed for step: {step.StepId}", ex);

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
    }
}