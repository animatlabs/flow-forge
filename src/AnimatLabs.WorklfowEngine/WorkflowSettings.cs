using System;

namespace AnimatLabs.WorkflowEngine
{
    /// <summary>
    /// Configuration settings for the workflow engine.
    /// </summary>
    public sealed class WorkflowSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowSettings"/> class.
        /// </summary>
        /// <param name="autoCompensate">Whether to automatically compensate steps on failure (default: true).</param>
        /// <param name="continueOnCompensationFailure">Whether to continue compensating other steps if one fails (default: true).</param>
        /// <param name="compensationRetries">Number of retries for compensation (default: 2).</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="compensationRetries"/> is less than or equal to 0.</exception>
        public WorkflowSettings(
            bool autoCompensate = true,
            bool continueOnCompensationFailure = true,
            int compensationRetries = 3)
        {
            AutoCompensate = autoCompensate;
            CompensationRetries = compensationRetries > 0 ? compensationRetries : throw new ArgumentOutOfRangeException(nameof(compensationRetries), "Value must be positive.");
            ContinueOnCompensationFailure = continueOnCompensationFailure;
        }

        /// <summary>
        /// Whether to automatically compensate steps on failure (default: true).
        /// </summary>
        public bool AutoCompensate { get; }

        /// <summary>
        /// Number of retries for compensation (default: 3).
        /// </summary>
        public int CompensationRetries { get; }

        /// <summary>
        /// Whether to continue compensating other steps if one fails (default: true).
        /// </summary>
        public bool ContinueOnCompensationFailure { get; }
    }
}