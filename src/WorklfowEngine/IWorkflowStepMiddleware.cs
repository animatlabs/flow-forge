using System;
using System.Threading;
using System.Threading.Tasks;

namespace WorkflowEngine
{
    /// <summary>
    /// Represents middleware that can intercept step execution.
    /// </summary>
    public interface IWorkflowStepMiddleware
    {
        /// <summary>
        /// Executes the middleware logic.
        /// </summary>
        /// <param name="workflowStep">The current workflow step.</param>
        /// <param name="context">The workflow context.</param>
        /// <param name="next">The delegate representing the next step in the workflow.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ExecuteAsync(
            IWorkflowStep workflowStep,
            IWorkflowContext context,
            Func<Task> next,
            CancellationToken cancellationToken
        );
    }
}