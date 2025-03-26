using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleWorkflowEngine.Core.RetryStrategy
{
    /// <summary>
    /// Abstract base class for retry strategies, providing common retry logic.
    /// </summary>
    public abstract class RetryStrategyBase : IRetryStrategy
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RetryStrategyBase"/> class.
        /// </summary>
        /// <param name="logger">The logger instance, or null for no logging.</param>
        protected RetryStrategyBase(ILogger logger = null)
        {
            this.logger = logger ?? new NullLogger();
        }

        /// <inheritdoc />
        public abstract Task<bool> ShouldRetryAsync(int retryCount, CancellationToken cancellationToken);

        /// <inheritdoc />
        public abstract TimeSpan GetRetryDelay(int retryCount);

        /// <inheritdoc />
        public virtual async Task ExecuteAsync(Func<Task> value, CancellationToken cancellationToken)
        {
            int retryCount = 0;

            while (true)
            {
                try
                {
                    await value();
                    return;
                }
                catch (Exception ex)
                {
                    retryCount++;

                    logger.LogWarning($"Retry {retryCount} failed.", ex);
                    if (!await ShouldRetryAsync(retryCount, cancellationToken))
                    {
                        throw;
                    }

                    var delay = GetRetryDelay(retryCount);
                    logger.LogInformation($"Retry {retryCount} failed. Retrying in {delay.TotalMilliseconds}ms. Exception: {ex.Message}", ex);
                    await Task.Delay(delay, cancellationToken);
                }
            }
        }
    }
}