using System;
using System.Threading;
using System.Threading.Tasks;
using AnimatLabs.WorkflowEngine.Exceptions;

namespace AnimatLabs.WorkflowEngine.Middlewares
{
    /// <summary>
    /// Middleware for handling errors during the execution of workflow steps.
    /// </summary>
    /// <remarks>
    /// This middleware intercepts the execution of workflow steps and handles any exceptions that occur,
    /// wrapping them in a <see cref="WorkflowException"/> and rethrowing them.
    /// </remarks>
    public class ErrorHandlingMiddleware : IWorkflowStepMiddleware
    {
        /// <inheritdoc />
        public async Task ExecuteAsync(
            IWorkflowStep workflowStep,
            IWorkflowContext context,
            Func<Task> next,
            CancellationToken cancellationToken)
        {
            try
            {
                await next();
            }
            catch (Exception ex)
            {
                // Log, retry, or rethrow
                throw new WorkflowException("Step failed.", ex);
            }
        }
    }
}
