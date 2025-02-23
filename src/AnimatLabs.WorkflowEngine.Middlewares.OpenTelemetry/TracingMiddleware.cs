using System;
using System.Threading;
using System.Threading.Tasks;
using AnimatLabs.WorkflowEngine;
using AnimatLabs.WorkflowEngine.Middlewares.OpenTelemetry;
using OpenTelemetry.Trace;

/// <summary>
/// Middleware for tracing workflow steps using OpenTelemetry.
/// </summary>
public class TracingMiddleware : IWorkflowStepMiddleware
{
    private readonly Tracer tracer;

    /// <summary>
    /// Initializes a new instance of the <see cref="TracingMiddleware"/> class.
    /// </summary>
    /// <param name="tracer">The OpenTelemetry tracer.</param>
    public TracingMiddleware(Tracer tracer)
    {
        this.tracer = tracer;
    }

    /// <inheritdoc />
    public async Task ExecuteAsync(
        IWorkflowStep step,
        IWorkflowContext context,
        Func<Task> next,
        CancellationToken ct
    )
    {
        using (var activity = tracer.StartActivity("ExecuteStep"))
        {
            activity?.SetTag("CorrelationId", context.CorrelationId);
            activity?.SetTag("WorkflowId", context.Workflow.WorkflowId);
            activity?.SetTag("WorkflowName", context.Workflow.Name);
            activity?.SetTag("StepId", step.StepId);
            activity?.SetTag("StepName", step.Name);
            await next();
        }
    }
}
