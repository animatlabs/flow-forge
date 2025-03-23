using System;

namespace WorkflowEngine.Core
{
    /// <summary>
    /// Interface representing a clock to provide current date and time.
    /// </summary>
    public interface ISystemClock
    {
        /// <summary>
        /// Gets the current date and time.
        /// </summary>
        DateTime Now { get; }

        /// <summary>
        /// Gets the current date and time in Coordinated Universal Time (UTC).
        /// </summary>
        DateTime UtcNow { get; }

        /// <summary>
        /// Gets the current date with the time component set to 00:00:00.
        /// </summary>
        DateTime Today { get; }
    }
}