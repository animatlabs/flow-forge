using System;
using System.Collections.Generic;

namespace SimpleWorkflowEngine.Core
{
    /// <summary>
    /// A logger implementation that does nothing. This is used as a fallback if no logging is required.
    /// </summary>
    internal sealed class NullLogger : ILogger
    {
        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state, IDictionary<string, string> properties = null)
        {
            return null;
        }

        /// <inheritdoc />
        public void LogCritical(string message, params object[] args)
        {
            // No operation
        }

        /// <inheritdoc />
        public void LogCritical(Exception exception, string message, params object[] args)
        {
            // No operation
        }

        /// <inheritdoc />
        public void LogCritical(IDictionary<string, string> properties, string message, params object[] args)
        {
            // No operation
        }

        /// <inheritdoc />
        public void LogCritical(IDictionary<string, string> properties, Exception exception, string message, params object[] args)
        {
            // No operation
        }

        /// <inheritdoc />
        public void LogDebug(string message, params object[] args)
        {
            // No operation
        }

        /// <inheritdoc />
        public void LogDebug(Exception exception, string message, params object[] args)
        {
            // No operation
        }

        /// <inheritdoc />
        public void LogDebug(IDictionary<string, string> properties, string message, params object[] args)
        {
            // No operation
        }

        /// <inheritdoc />
        public void LogDebug(IDictionary<string, string> properties, Exception exception, string message, params object[] args)
        {
            // No operation
        }

        /// <inheritdoc />
        public void LogError(string message, params object[] args)
        {
            // No operation
        }

        /// <inheritdoc />
        public void LogError(Exception exception, string message, params object[] args)
        {
            // No operation
        }

        /// <inheritdoc />
        public void LogError(IDictionary<string, string> properties, string message, params object[] args)
        {
            // No operation
        }

        /// <inheritdoc />
        public void LogError(IDictionary<string, string> properties, Exception exception, string message, params object[] args)
        {
            // No operation
        }

        /// <inheritdoc />
        public void LogInformation(string message, params object[] args)
        {
            // No operation
        }

        /// <inheritdoc />
        public void LogInformation(Exception exception, string message, params object[] args)
        {
            // No operation
        }

        /// <inheritdoc />
        public void LogInformation(IDictionary<string, string> properties, string message, params object[] args)
        {
            // No operation
        }

        /// <inheritdoc />
        public void LogInformation(IDictionary<string, string> properties, Exception exception, string message, params object[] args)
        {
            // No operation
        }

        /// <inheritdoc />
        public void LogTrace(string message, params object[] args)
        {
            // No operation
        }

        /// <inheritdoc />
        public void LogTrace(Exception exception, string message, params object[] args)
        {
            // No operation
        }

        /// <inheritdoc />
        public void LogTrace(IDictionary<string, string> properties, string message, params object[] args)
        {
            // No operation
        }

        /// <inheritdoc />
        public void LogTrace(IDictionary<string, string> properties, Exception exception, string message, params object[] args)
        {
            // No operation
        }

        /// <inheritdoc />
        public void LogWarning(string message, params object[] args)
        {
            // No operation
        }

        /// <inheritdoc />
        public void LogWarning(Exception exception, string message, params object[] args)
        {
            // No operation
        }

        /// <inheritdoc />
        public void LogWarning(IDictionary<string, string> properties, string message, params object[] args)
        {
            // No operation
        }

        /// <inheritdoc />
        public void LogWarning(IDictionary<string, string> properties, Exception exception, string message, params object[] args)
        {
            // No operation
        }
    }
}