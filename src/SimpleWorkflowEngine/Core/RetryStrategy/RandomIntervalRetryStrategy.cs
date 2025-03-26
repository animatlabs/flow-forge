using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleWorkflowEngine.Core.RetryStrategy
{
    /// <summary>
    /// Implements a retry strategy with random intervals between retries.
    /// </summary>
    public class RandomIntervalRetryStrategy : RetryStrategyBase
    {
        private readonly TimeSpan minInterval;
        private readonly TimeSpan maxInterval;
        private readonly Random random;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomIntervalRetryStrategy"/> class.
        /// </summary>
        /// <param name="minInterval">The minimum interval between retries.</param>
        /// <param name="maxInterval">The maximum interval between retries.</param>
        /// <param name="logger">The logger instance, or null for no logging.</param>
        public RandomIntervalRetryStrategy(TimeSpan minInterval, TimeSpan maxInterval, ILogger logger = null)
            : base(logger)
        {
            this.minInterval = minInterval;
            this.maxInterval = maxInterval;
            random = new Random();
        }

        /// <inheritdoc />
        public override Task<bool> ShouldRetryAsync(int retryCount, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        /// <inheritdoc />
        public override TimeSpan GetRetryDelay(int retryCount)
        {
            var delay = minInterval.TotalMilliseconds + random.NextDouble() * (maxInterval.TotalMilliseconds - minInterval.TotalMilliseconds);
            return TimeSpan.FromMilliseconds(delay);
        }
    }
}