using System;
using System.Threading;
using System.Threading.Tasks;

namespace WorkflowEngine.Middlewares
{
    /// <summary>
    /// Fails a step if it exceeds a specified duration (optional middleware).
    /// </summary>
    public class TimeoutMiddleware : IWorkflowStepMiddleware
    {
        private readonly TimeSpan timeout;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeoutMiddleware"/> class.
        /// </summary>
        /// <param name="timeout">The timeout duration.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="timeout"/> is less than zero.</exception>
        public TimeoutMiddleware(TimeSpan timeout)
        {
            this.timeout = timeout >= TimeSpan.Zero ? timeout : throw new ArgumentOutOfRangeException(nameof(timeout));
        }

        /// <inheritdoc/>
        public async Task ExecuteAsync(
            IWorkflowStep step,
            IWorkflowContext context,
            Func<Task> next,
            CancellationToken cancellationToken
        )
        {
            using (var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                cancellationTokenSource.CancelAfter(timeout);
                await next().ConfigureAwait(false);
            }
        }
    }
}