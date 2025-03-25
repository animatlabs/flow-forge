using System;
using System.Threading;
using System.Threading.Tasks;

namespace WorkflowEngine
{
    /// <summary>
    /// Represents a workflow step with execution and compensation capabilities.
    /// </summary>
    public interface IWorkflowStep : IDisposable
    {
        /// <summary>
        /// Gets the unique identifier for the step.
        /// </summary>
        Guid StepId { get; }

        /// <summary>
        /// Gets the name of the workflow step.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Executes the step's logic asynchronously.
        /// </summary>
        /// <param name="context">The workflow context.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ExecuteAsync(IWorkflowContext context, CancellationToken cancellationToken);

        /// <summary>
        /// Compensates for the step's actions asynchronously.
        /// </summary>
        /// <param name="context">The workflow context.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task CompensateAsync(IWorkflowContext context, CancellationToken cancellationToken);
    }
}