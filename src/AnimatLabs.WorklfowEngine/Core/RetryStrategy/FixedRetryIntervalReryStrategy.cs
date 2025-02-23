using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnimatLabs.WorkflowEngine.Core.RetryStrategy
{
    /// <summary>
    /// Implements a fixed interval retry strategy.
    /// </summary>
    public sealed class FixedIntervalRetryStrategy : IRetryStrategy
    {
        private readonly TimeSpan retryInterval;
        private readonly int maxRetryCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="FixedIntervalRetryStrategy"/> class.
        /// </summary>
        /// <param name="retryInterval">The interval between retries.</param>
        /// <param name="maxRetryCount">The maximum number of retry attempts.</param>
        public FixedIntervalRetryStrategy(TimeSpan retryInterval, int maxRetryCount)
        {
            this.retryInterval = retryInterval;
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
            return retryInterval;
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
                    if (retryCount >= maxRetryCount || cancellationToken.IsCancellationRequested)
                    {
                        throw;
                    }
                    await Task.Delay(retryInterval, cancellationToken);
                }
            }
        }
    }
}
