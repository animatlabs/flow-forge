using System;
using System.Threading;
using System.Threading.Tasks;

namespace WorkflowEngine.Core.RetryStrategy
{
    /// <summary>
    /// Implements a fixed interval retry strategy.
    /// </summary>
    public sealed class FixedIntervalRetryStrategy : RetryStrategyBase
    {
        private readonly TimeSpan retryInterval;
        private readonly int maxRetryCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="FixedIntervalRetryStrategy"/> class.
        /// </summary>
        /// <param name="retryInterval">The interval between retries.</param>
        /// <param name="maxRetryCount">The maximum number of retry attempts.</param>
        /// <param name="logger">The logger instance, or null for no logging.</param>
        public FixedIntervalRetryStrategy(TimeSpan retryInterval, int maxRetryCount, ILogger logger = null)
            : base(logger)
        {
            this.retryInterval = retryInterval;
            this.maxRetryCount = maxRetryCount;
        }

        /// <inheritdoc />
        public override Task<bool> ShouldRetryAsync(int retryCount, CancellationToken cancellationToken)
        {
            return Task.FromResult(retryCount < maxRetryCount);
        }

        /// <inheritdoc />
        public override TimeSpan GetRetryDelay(int retryCount)
        {
            return retryInterval;
        }
    }
}