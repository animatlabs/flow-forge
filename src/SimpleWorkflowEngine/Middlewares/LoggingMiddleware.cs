using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SimpleWorkflowEngine.Core;

namespace SimpleWorkflowEngine.Middlewares
{
    /// <summary>
    /// Middleware that logs the execution of workflow steps.
    /// </summary>
    public class LoggingMiddleware : IWorkflowStepMiddleware
    {
        private readonly ILogger logger;
        private readonly ISystemClock systemClock;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingMiddleware"/> class.
        /// </summary>
        /// <param name="logger">The logger to use for logging step execution.</param>
        /// <param name="systemClock">The system clock to use for getting the current time (optional).</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="logger"/> is null.</exception>
        public LoggingMiddleware(ILogger logger, ISystemClock systemClock = null)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.systemClock = systemClock ?? new SystemClock();
        }

        /// <inheritdoc/>
        public async Task ExecuteAsync(
            IWorkflowStep step,
            IWorkflowContext context,
            Func<Task> next,
            CancellationToken cancellationToken
        )
        {
            var startTime = systemClock.UtcNow;
            try
            {
                logger.LogDebug("Starting step");
                await next();
            }
            finally
            {
                logger.LogInformation(new Dictionary<string, string> { { "Duration", $"{(startTime - systemClock.UtcNow).TotalMilliseconds}" } }, "Completed Step");
            }
        }
    }
}