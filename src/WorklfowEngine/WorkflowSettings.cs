using System;

namespace WorkflowEngine
{
    /// <summary>
    /// Configuration settings for the workflow engine.
    /// </summary>
    public sealed class WorkflowSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowSettings"/> class.
        /// </summary>
        /// <param name="autoCompensate">Indicates whether the workflow engine should automatically compensate on failure.</param>
        /// <param name="continueOnCompensationFailure">Indicates whether the workflow engine should continue on compensation failure.</param>
        /// <param name="compensationRetries">The number of retries for compensation.</param>
        /// <param name="maxConcurrentWorkflows">The maximum number of concurrent workflows.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="compensationRetries"/> or <paramref name="maxConcurrentWorkflows"/> is less than or equal to 0.
        /// </exception>
        public WorkflowSettings(
            bool autoCompensate = true,
            bool continueOnCompensationFailure = true,
            int compensationRetries = 3,
            int maxConcurrentWorkflows = 10) // Default to 10 concurrent workflows
        {
            if (compensationRetries <= 0)
                throw new ArgumentOutOfRangeException(nameof(compensationRetries), "Compensation retries must be greater than 0.");
            if (maxConcurrentWorkflows <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxConcurrentWorkflows), "Max concurrent workflows must be greater than 0.");

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
        /// Gets the number of retries for compensation.
        /// </summary>
        public int CompensationRetries { get; }

        /// <summary>
        /// Gets a value indicating whether the workflow engine should continue on compensation failure.
        /// </summary>
        public bool ContinueOnCompensationFailure { get; }

        /// <summary>
        /// Gets the maximum number of concurrent workflows.
        /// </summary>
        public int MaxConcurrentWorkflows { get; }
    }
}