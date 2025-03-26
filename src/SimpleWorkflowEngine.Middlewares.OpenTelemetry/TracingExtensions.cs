using System.Diagnostics;
using OpenTelemetry.Trace;

namespace SimpleWorkflowEngine.Middlewares.OpenTelemetry
{
    /// <summary>
    /// Provides extension methods for tracing activities.
    /// </summary>
    internal static class TracingExtensions
    {
        /// <summary>
        /// Starts a new activity with the specified name.
        /// </summary>
        /// <param name="tracer">The tracer to use for starting the activity.</param>
        /// <param name="name">The name of the activity.</param>
        /// <returns>The started activity.</returns>
        public static Activity StartActivity(this Tracer tracer, string name)
        => tracer.StartActivity(name);
    }
}