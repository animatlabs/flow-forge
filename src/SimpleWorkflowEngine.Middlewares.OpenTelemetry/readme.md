# Simple Workflow Engine OpenTelemetry Middleware

The `SimpleWorkflowEngine.Middlewares.OpenTelemetry` project provides OpenTelemetry integration for the `SimpleWorkflowEngine`. It enables distributed tracing for workflows and steps.

## Features

- **Distributed Tracing**: Trace workflow execution across services.
- **Step-Level Metadata**: Attach workflow and step identifiers to traces.
- **OpenTelemetry Integration**: Leverage OpenTelemetry for observability.

## Getting Started

1. **Install OpenTelemetry**: Add OpenTelemetry to your project.
2. **Configure Tracer**: Create an OpenTelemetry tracer.
3. **Use Middleware**: Add `TracingMiddleware` to the workflow runner.

## Example

```csharp
var tracer = TracerProvider.Default.GetTracer("SimpleWorkflowEngine");
var middleware = new TracingMiddleware(tracer);

var workflow = new WorkflowBuilder()
    .WithId(Guid.NewGuid())
    .WithName("Sample Workflow")
    .AddStep(new SampleStep())
    .Build();

var runner = new WorkflowRunner(new[] { middleware });
await runner.ExecuteAsync(workflow, new WorkflowContext(workflow));
```

## License

This project is licensed under the MIT License.
