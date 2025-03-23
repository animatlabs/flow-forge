using System;
using System.Threading;
using System.Threading.Tasks;

namespace WorkflowEngine.Core.RetryStrategy
{
    /// <summary>
    /// Implements a random interval retry strategy.
    /// </summary>
    public class RandomIntervalRetryStrategy : IRetryStrategy
    {
        private readonly TimeSpan minInterval;
        private readonly TimeSpan maxInterval;
        private readonly Random random;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomIntervalRetryStrategy"/> class.
        /// </summary>
        /// <param name="minInterval">The minimum interval between retries.</param>
        /// <param name="maxInterval">The maximum interval between retries.</param>
        public RandomIntervalRetryStrategy(TimeSpan minInterval, TimeSpan maxInterval)
        {
            this.minInterval = minInterval;
            this.maxInterval = maxInterval;
            random = new Random();
        }

        /// <inheritdoc />
        public Task<bool> ShouldRetryAsync(int retryCount, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        /// <inheritdoc />
        public TimeSpan GetRetryDelay(int retryCount)
        {
            var delay = minInterval.TotalMilliseconds + random.NextDouble() * (maxInterval.TotalMilliseconds - minInterval.TotalMilliseconds);
            return TimeSpan.FromMilliseconds(delay);
        }

        /// <summary>
        /// Executes the provided asynchronous operation with retry logic.
        /// </summary>
        /// <param name="value">The asynchronous operation to execute.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
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
                    await Task.Delay(GetRetryDelay(retryCount), cancellationToken);
                }
            }
        }
    }
}