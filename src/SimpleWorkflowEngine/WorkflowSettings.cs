using System;

namespace SimpleWorkflowEngine
{
    /// <summary>
    /// Configuration settings for the workflow engine.
    /// </summary>
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
        /// <param name="compensationRetries">The number of retries for compensation.</param>
        /// <param name="maxConcurrentWorkflows">The maximum number of concurrent workflows.</param>
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

        /// <summary>
        /// Gets a value indicating whether the workflow engine should automatically compensate on failure.
        /// </summary>
        public bool AutoCompensate { get; }

        /// <summary>
        /// Gets a value indicating whether the workflow engine should continue execution on compensation failure.
        /// </summary>
        public bool ContinueOnCompensationFailure { get; }

        /// <summary>
        /// Gets the number of retries for compensation.
        /// </summary>
        public int CompensationRetries { get; }

        /// <summary>
        /// Gets the maximum number of workflows that can run concurrently.
        /// </summary>
        public int MaxConcurrentWorkflows { get; }
    }
}