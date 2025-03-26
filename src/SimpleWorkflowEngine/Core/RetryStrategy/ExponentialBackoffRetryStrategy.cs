using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleWorkflowEngine.Core.RetryStrategy
{
    /// <summary>
    /// Implements a retry strategy with exponentially increasing intervals between retries.
    /// </summary>
    public class ExponentialBackoffRetryStrategy : RetryStrategyBase
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
        /// <param name="logger">The logger instance, or null for no logging.</param>
        public ExponentialBackoffRetryStrategy(TimeSpan initialInterval, TimeSpan maxInterval, int maxRetryCount, ILogger logger = null)
            : base(logger)
        {
            this.initialInterval = initialInterval;
            this.maxInterval = maxInterval;
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
            var delay = TimeSpan.FromMilliseconds(initialInterval.TotalMilliseconds * Math.Pow(2, retryCount));
            return delay < maxInterval ? delay : maxInterval;
        }
    }
}