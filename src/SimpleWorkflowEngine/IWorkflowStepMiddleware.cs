using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleWorkflowEngine
{
    /// <summary>
    /// Represents middleware that can intercept and extend the execution of workflow steps.
    /// </summary>
    public interface IWorkflowStepMiddleware
    {
        /// <summary>
        /// Executes the middleware logic.
        /// </summary>
        /// <param name="workflowStep">The current workflow step being executed.</param>
        /// <param name="context">The workflow context containing shared data and services.</param>
        /// <param name="next">The delegate representing the next step in the workflow pipeline.</param>
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