using System;
using System.Collections.Generic;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Core.Enrichers;

namespace AnimatLabs.WorkflowEngine.Logger.Serilog
{
    /// <summary>
    /// Logger class that implements the ILogger interface.
    /// </summary>
    public class Logger : Core.ILogger
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        /// <param name="logger">The Serilog logger instance.</param>
        public Logger(ILogger logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state, IDictionary<string, string> properties = null)
        {
            return PushProperties(properties);
        }

        /// <inheritdoc />
        public void LogCritical(string message, params object[] args)
        {
            logger.Fatal(message, args);
        }

        /// <inheritdoc />
        public void LogCritical(Exception exception, string message, params object[] args)
        {
            logger.Fatal(exception, message, args);
        }

        /// <inheritdoc />
        public void LogCritical(IDictionary<string, string> properties, string message, params object[] args)
        {
            using (PushProperties(properties))
            {
                logger.Fatal(message, args);
            }
        }

        /// <inheritdoc />
        public void LogCritical(IDictionary<string, string> properties, Exception exception, string message, params object[] args)
        {
            using (PushProperties(properties))
            {
                logger.Fatal(exception, message, args);
            }
        }

        /// <inheritdoc />
        public void LogDebug(string message, params object[] args)
        {
            logger.Debug(message, args);
        }

        /// <inheritdoc />
        public void LogDebug(Exception exception, string message, params object[] args)
        {
            logger.Debug(exception, message, args);
        }

        /// <inheritdoc />
        public void LogDebug(IDictionary<string, string> properties, string message, params object[] args)
        {
            using (PushProperties(properties))
            {
                logger.Debug(message, args);
            }
        }

        /// <inheritdoc />
        public void LogDebug(IDictionary<string, string> properties, Exception exception, string message, params object[] args)
        {
            using (PushProperties(properties))
            {
                logger.Debug(exception, message, args);
            }
        }

        /// <inheritdoc />
        public void LogError(string message, params object[] args)
        {
            logger.Error(message, args);
        }

        /// <inheritdoc />
        public void LogError(Exception exception, string message, params object[] args)
        {
            logger.Error(exception, message, args);
        }

        /// <inheritdoc />
        public void LogError(IDictionary<string, string> properties, string message, params object[] args)
        {
            using (PushProperties(properties))
            {
                logger.Error(message, args);
            }
        }

        /// <inheritdoc />
        public void LogError(IDictionary<string, string> properties, Exception exception, string message, params object[] args)
        {
            using (PushProperties(properties))
            {
                logger.Error(exception, message, args);
            }
        }

        /// <inheritdoc />
        public void LogInformation(string message, params object[] args)
        {
            logger.Information(message, args);
        }

        /// <inheritdoc />
        public void LogInformation(Exception exception, string message, params object[] args)
        {
            logger.Information(exception, message, args);
        }

        /// <inheritdoc />
        public void LogInformation(IDictionary<string, string> properties, string message, params object[] args)
        {
            using (PushProperties(properties))
            {
                logger.Information(message, args);
            }
        }

        /// <inheritdoc />
        public void LogInformation(IDictionary<string, string> properties, Exception exception, string message, params object[] args)
        {
            using (PushProperties(properties))
            {
                logger.Information(exception, message, args);
            }
        }

        /// <inheritdoc />
        public void LogTrace(string message, params object[] args)
        {
            logger.Verbose(message, args);
        }

        /// <inheritdoc />
        public void LogTrace(Exception exception, string message, params object[] args)
        {
            logger.Verbose(exception, message, args);
        }

        /// <inheritdoc />
        public void LogTrace(IDictionary<string, string> properties, string message, params object[] args)
        {
            using (PushProperties(properties))
            {
                logger.Verbose(message, args);
            }
        }

        /// <inheritdoc />
        public void LogTrace(IDictionary<string, string> properties, Exception exception, string message, params object[] args)
        {
            using (PushProperties(properties))
            {
                logger.Verbose(exception, message, args);
            }
        }

        /// <inheritdoc />
        public void LogWarning(string message, params object[] args)
        {
            logger.Warning(message, args);
        }

        /// <inheritdoc />
        public void LogWarning(Exception exception, string message, params object[] args)
        {
            logger.Warning(exception, message, args);
        }

        /// <inheritdoc />
        public void LogWarning(IDictionary<string, string> properties, string message, params object[] args)
        {
            using (PushProperties(properties))
            {
                logger.Warning(message, args);
            }
        }

        /// <inheritdoc />
        public void LogWarning(IDictionary<string, string> properties, Exception exception, string message, params object[] args)
        {
            using (PushProperties(properties))
            {
                logger.Warning(exception, message, args);
            }
        }

        private IDisposable PushProperties(IDictionary<string, string> properties)
        {
            var enrichers = new List<ILogEventEnricher>();
            foreach (var property in properties)
            {
                enrichers.Add(new PropertyEnricher(property.Key, property.Value));
            }
            return LogContext.Push(enrichers.ToArray());
        }
    }
}
