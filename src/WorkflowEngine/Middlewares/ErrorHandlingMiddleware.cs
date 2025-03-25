using System;
using System.Threading;
using System.Threading.Tasks;
using WorkflowEngine.Core;
using WorkflowEngine.Exceptions;

namespace WorkflowEngine.Middlewares
{
    /// <summary>
    /// Middleware for handling errors during the execution of workflow steps.
    /// </summary>
    public class ErrorHandlingMiddleware : IWorkflowStepMiddleware
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandlingMiddleware"/> class.
        /// </summary>
        /// <param name="logger">The logger instance to use for logging errors.</param>
        public ErrorHandlingMiddleware(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Executes the middleware logic.
        /// </summary>
        /// <param name="workflowStep">The workflow step being executed.</param>
        /// <param name="context">The workflow context.</param>
        /// <param name="next">The next middleware to execute.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
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
                logger.LogError($"Unhandled exception in step {workflowStep.StepId}: {ex.Message}", ex);
                throw new WorkflowException("Step execution failed.", ex);
            }
        }
    }
}