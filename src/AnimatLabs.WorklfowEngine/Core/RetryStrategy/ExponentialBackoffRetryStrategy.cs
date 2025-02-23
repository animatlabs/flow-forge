using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnimatLabs.WorkflowEngine.Core.RetryStrategy
{
    /// <summary>
    /// Implements an exponential backoff retry strategy.
    /// </summary>
    public class ExponentialBackoffRetryStrategy : IRetryStrategy
    {
        private readonly TimeSpan initialInterval;
        private readonly TimeSpan maxInterval;
        private readonly int maxRetryCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExponentialBackoffRetryStrategy"/> class.
        /// </summary>
        /// <param name="initialInterval">The initial interval between retries.</param>
        /// <param name="maxInterval">The maximum interval between retries.</param>
        /// <param name="maxRetryCount">The maximum number of retry attempts.</param>
        public ExponentialBackoffRetryStrategy(TimeSpan initialInterval, TimeSpan maxInterval, int maxRetryCount)
        {
            this.initialInterval = initialInterval;
            this.maxInterval = maxInterval;
            this.maxRetryCount = maxRetryCount;
        }

        /// <inheritdoc />
        public Task<bool> ShouldRetryAsync(int retryCount, CancellationToken cancellationToken)
        {
            return Task.FromResult(retryCount < maxRetryCount);
        }

        /// <inheritdoc />
        public TimeSpan GetRetryDelay(int retryCount)
        {
            var delay = TimeSpan.FromMilliseconds(initialInterval.TotalMilliseconds * Math.Pow(2, retryCount));
            return delay < maxInterval ? delay : maxInterval;
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(Func<Task> value, CancellationToken cancellationToken)
        {
            int retryCount = 0;

            while (true)
            {
                try
                {
                    await value();
                    return;
                }
                catch (Exception)
                {
                    retryCount++;

                    if (!await ShouldRetryAsync(retryCount, cancellationToken))
                    {
                        throw;
                    }

                    var delay = GetRetryDelay(retryCount);
                    await Task.Delay(delay, cancellationToken);
                }
            }
        }
    }
}
