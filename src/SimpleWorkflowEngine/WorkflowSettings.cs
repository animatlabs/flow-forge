using System;

namespace SimpleWorkflowEngine
{
    /// <summary>
    /// Configuration settings for the workflow engine.
    /// </summary>
    /// <remarks>
    /// These settings control the behavior of the workflow engine, including compensation retries,
    /// concurrency limits, and error handling strategies.
    /// </remarks>
    public sealed class WorkflowSettings
    {
        private const int minAllowedCompensationRetries = 1;
        private const int maxAllowedCompensationRetries = 100;
        private const int minAllowedConcurrentWorkflows = 1;
        private const int maxAllowedConcurrentWorkflows = 1000;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowSettings"/> class.
        /// </summary>
        /// <param name="autoCompensate">Indicates whether the workflow engine should automatically compensate on failure.</param>
        /// <param name="continueOnCompensationFailure">Indicates whether the workflow engine should continue on compensation failure.</param>
        /// <param name="compensationRetries">
        /// The number of retries for compensation. Must be between 1 and 100.
        /// </param>
        /// <param name="maxConcurrentWorkflows">
        /// The maximum number of concurrent workflows. Must be between 1 and 1000.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="compensationRetries"/> or <paramref name="maxConcurrentWorkflows"/> is out of range.
        /// </exception>
        public WorkflowSettings(
            bool autoCompensate = true,
            bool continueOnCompensationFailure = true,
            int compensationRetries = 3,
            int maxConcurrentWorkflows = 10)
        {
            if (compensationRetries < minAllowedCompensationRetries || compensationRetries > maxAllowedCompensationRetries)
            {
                throw new ArgumentOutOfRangeException(nameof(compensationRetries), $"Compensation retries must be between {minAllowedCompensationRetries} and {maxAllowedCompensationRetries}.");
            }

            if (maxConcurrentWorkflows < minAllowedConcurrentWorkflows || maxConcurrentWorkflows > maxAllowedConcurrentWorkflows)
            {
                throw new ArgumentOutOfRangeException(nameof(maxConcurrentWorkflows), $"Max concurrent workflows must be between {minAllowedConcurrentWorkflows} and {maxAllowedConcurrentWorkflows}.");
            }

            AutoCompensate = autoCompensate;
            ContinueOnCompensationFailure = continueOnCompensationFailure;
            CompensationRetries = compensationRetries;
            MaxConcurrentWorkflows = maxConcurrentWorkflows;
        }

        /// <inheritdoc />
        public bool AutoCompensate { get; }

        /// <inheritdoc />
        public bool ContinueOnCompensationFailure { get; }

        /// <inheritdoc />
        public int CompensationRetries { get; }

        /// <inheritdoc />
        public int MaxConcurrentWorkflows { get; }
    }
}