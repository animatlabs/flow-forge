using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnimatLabs.WorkflowEngine.Core
{
    /// <summary>
    /// Defines a strategy for retrying operations.
    /// </summary>
    public interface IRetryStrategy
    {
        /// <summary>
        /// Determines whether a retry should be attempted.
        /// </summary>
        /// <param name="retryCount">The current retry count.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether a retry should be attempted.</returns>
        Task<bool> ShouldRetryAsync(int retryCount, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the delay before the next retry attempt.
        /// </summary>
        /// <param name="retryCount">The current retry count.</param>
        /// <returns>The delay before the next retry attempt.</returns>
        TimeSpan GetRetryDelay(int retryCount);

        /// <summary>
        /// Executes the provided asynchronous operation with retry logic.
        /// </summary>
        /// <param name="value">The asynchronous operation to execute.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ExecuteAsync(Func<Task> value, CancellationToken cancellationToken);
    }
}
