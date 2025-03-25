using System;
using System.Threading;
using System.Threading.Tasks;
using WorkflowEngine.Core;

namespace WorkflowEngine
{
    /// <summary>
    /// Represents a base class for workflow steps with default execution and compensation logic.
    /// </summary>
    public abstract class WorkflowStep : IWorkflowStep
    {
        /// <summary>
        /// The logger instance to use for logging.
        /// </summary>
        protected readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowStep"/> class.
        /// </summary>
        /// <param name="logger">The logger instance to use for logging.</param>
        /// <param name="name">The name of the workflow step. If not provided, the class name will be used.</param>
        protected WorkflowStep(ILogger logger = null, string name = null)
        {
            this.logger = logger ?? new NullLogger();
            Name = GetType().Name;
        }

        /// <inheritdoc/>
        public Guid StepId { get; } = Guid.NewGuid();

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public virtual async Task ExecuteAsync(
            IWorkflowContext context,
            CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();
            await ExecuteCoreAsync(context, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public virtual async Task CompensateAsync(
            IWorkflowContext context,
            CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();
            await CompensateCoreAsync(context, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Core execution logic (override in derived classes).
        /// </summary>
        /// <param name="context">The workflow context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected abstract Task ExecuteCoreAsync(
            IWorkflowContext context,
            CancellationToken cancellationToken
        );

        /// <summary>
        /// Core compensation logic (override if needed).
        /// </summary>
        /// <param name="context">The workflow context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected virtual Task CompensateCoreAsync(
            IWorkflowContext context,
            CancellationToken cancellationToken
        ) => Task.CompletedTask;

        /// <inheritdoc/>
        public virtual void Dispose()
        { }
    }
}