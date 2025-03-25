using System;
using System.Threading;
using System.Threading.Tasks;

namespace WorkflowEngine.Middlewares
{
    /// <summary>
    /// Retries failed steps a specified number of times.
    /// </summary>
    public class RetryMiddleware : IWorkflowStepMiddleware
    {
        private readonly int maxRetries;
        private readonly TimeSpan delay;

        /// <summary>
        /// Initializes a new instance of the <see cref="RetryMiddleware"/> class.
        /// </summary>
        /// <param name="maxRetries">The maximum number of retry attempts.</param>
        /// <param name="delay">The delay between retry attempts.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="maxRetries"/> is less than 0 or <paramref name="delay"/> is less than <see cref="TimeSpan.Zero"/>.
        /// </exception>
        public RetryMiddleware(int maxRetries, TimeSpan delay)
        {
            this.maxRetries = maxRetries >= 0 ? maxRetries : throw new ArgumentOutOfRangeException(nameof(maxRetries));
            this.delay = delay >= TimeSpan.Zero ? delay : throw new ArgumentOutOfRangeException(nameof(delay));
        }

        /// <inheritdoc/>
        public async Task ExecuteAsync(
            IWorkflowStep step,
            IWorkflowContext context,
            Func<Task> next,
            CancellationToken cancellationToken
        )
        {
            for (var retryCount = 0; retryCount < maxRetries; retryCount++)
            {
                try
                {
                    await next();
                    return;
                }
                catch (Exception) when (retryCount < maxRetries - 1)
                {
                    await Task.Delay(delay, cancellationToken);
                }
            }
        }
    }
}