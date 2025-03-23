using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkflowEngine.Core;

namespace WorkflowEngine.Steps
{
    /// <summary>
    /// Represents a step that can execute multiple workflow steps in parallel, if so configured.
    /// </summary>
    public sealed class ForeachStep : WorkflowStep
    {
        private readonly ConcurrentBag<IWorkflowStep> parallelSteps;
        private readonly SemaphoreSlim throttler;
        private readonly bool useThrottler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForeachStep"/> class.
        /// </summary>
        /// <param name="maxDegreeOfParallelism">The maximum number of parallel tasks (default: 1).</param>
        /// <param name="logger">The logger instance to use for logging.</param>
        public ForeachStep(int maxDegreeOfParallelism = 1, ILogger logger = null)
            : base(logger)
        {
            if (maxDegreeOfParallelism < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxDegreeOfParallelism), "maxDegreeOfParallelism must be greater than or equal to 0.");
            }
            parallelSteps = new ConcurrentBag<IWorkflowStep>();
            useThrottler = maxDegreeOfParallelism > 0;
            throttler = useThrottler ? new SemaphoreSlim(maxDegreeOfParallelism) : null;
        }

        /// <summary>
        /// Adds a workflow step to be executed.
        /// </summary>
        /// <param name="step">The workflow step to add.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="step"/> is null.</exception>
        public void AddStep(IWorkflowStep step) => parallelSteps.Add(step ?? throw new ArgumentNullException(nameof(step)));

        /// <inheritdoc/>
        protected override async Task ExecuteCoreAsync(IWorkflowContext context, CancellationToken cancellationToken)
        {
            var tasks = parallelSteps.Select(async step =>
            {
                using (_ = logger.BeginScope(new Dictionary<string, object> {
                    { "ParentStepId", StepId },
                    { "ParentStepName", Name },
                    { "StepId", step.StepId },
                    { "StepName", step.Name }
                }))
                {
                    if (useThrottler)
                    {
                        await throttler.WaitAsync(cancellationToken);
                    }
                    try
                    {
                        await step.ExecuteAsync(context, cancellationToken);
                    }
                    finally
                    {
                        if (useThrottler)
                        {
                            throttler.Release();
                        }
                    }
                }
            });

            await Task.WhenAll(tasks);
        }

        /// <inheritdoc/>
        protected override async Task CompensateCoreAsync(IWorkflowContext context, CancellationToken cancellationToken)
        {
            var tasks = parallelSteps.Select(async step =>
            {
                using (_ = logger.BeginScope(new Dictionary<string, object> {
                    { "ParentStepId", StepId },
                    { "ParentStepName", Name },
                    { "StepId", step.StepId },
                    { "StepName", step.Name }
                }))
                {
                    if (useThrottler)
                    {
                        await throttler.WaitAsync(cancellationToken);
                    }
                    try
                    {
                        await step.CompensateAsync(context, cancellationToken);
                    }
                    finally
                    {
                        if (useThrottler)
                        {
                            throttler.Release();
                        }
                    }
                }
            });

            await Task.WhenAll(tasks);
        }

        /// <inheritdoc/>
        public override void Dispose()
        {
            foreach (var step in parallelSteps)
            {
                step.Dispose();
            }
            throttler?.Dispose();
        }
    }
}