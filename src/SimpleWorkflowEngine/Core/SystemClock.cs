using System;

namespace SimpleWorkflowEngine.Core
{
    /// <summary>
    /// Provides the current date and time.
    /// </summary>
    internal sealed class SystemClock : ISystemClock
    {
        /// <summary>
        /// Gets the current date and time.
        /// </summary>
        public DateTime Now => DateTime.Now;

        /// <summary>
        /// Gets the current date and time in Coordinated Universal Time (UTC).
        /// </summary>
        public DateTime UtcNow => DateTime.UtcNow;

        /// <summary>
        /// Gets the current date with the time component set to 00:00:00.
        /// </summary>
        public DateTime Today => DateTime.Today;
    }
}