using System;
using System.Threading;
using System.Threading.Tasks;
using OpenTelemetry.Trace;
using WorkflowEngine;
using WorkflowEngine.Middlewares.OpenTelemetry;

/// <summary>
/// Middleware for tracing workflow steps using OpenTelemetry.
/// </summary>
/// <remarks>
/// This middleware creates a tracing activity for each workflow step execution
/// and attaches relevant metadata such as workflow and step identifiers.
/// </remarks>
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