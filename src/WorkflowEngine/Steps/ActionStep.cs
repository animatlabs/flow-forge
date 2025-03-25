using System;
using System.Threading;
using System.Threading.Tasks;
using WorkflowEngine.Core;

namespace WorkflowEngine.Steps
{
    /// <summary>
    /// Represents a simple action-based workflow step with optional compensation.
    /// </summary>
    public sealed class ActionStep : WorkflowStep
    {
        private readonly Func<IWorkflowContext, CancellationToken, Task> action;
        private readonly Func<IWorkflowContext, CancellationToken, Task> compensation;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionStep"/> class.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="compensation">The compensation action to execute if needed.</param>
        /// <param name="logger">The logger instance to use for logging.</param>
        public ActionStep(
            Func<IWorkflowContext, CancellationToken, Task> action,
            Func<IWorkflowContext, CancellationToken, Task> compensation = null,
            ILogger logger = null)
            : base(logger)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
            this.compensation = compensation;
        }

        /// <inheritdoc/>
        protected override async Task ExecuteCoreAsync(IWorkflowContext context, CancellationToken cancellationToken)
        {
            await action(context, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        protected override async Task CompensateCoreAsync(IWorkflowContext context, CancellationToken cancellationToken)
        {
            if (compensation != null)
                await compensation(context, cancellationToken).ConfigureAwait(false);
        }
    }
}