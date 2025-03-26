# Simple Workflow Engine Serilog Logger

The `SimpleWorkflowEngine.Logger.Serilog` project provides seamless integration of Serilog with the `SimpleWorkflowEngine`. It enables detailed logging of workflow execution and step-level events, enhancing observability and debugging capabilities.

## Features

- **Step-Level Logging**: Log the execution and compensation of workflow steps with detailed information.
- **Custom Properties**: Add custom properties to logs for better traceability and context.
- **Serilog Integration**: Leverage Serilog's powerful logging capabilities, including structured logging and sinks.

## Getting Started

1. **Install Serilog**: Add Serilog to your project using NuGet.
2. **Configure Logger**: Create a `SerilogLogger` instance and configure it as needed.
3. **Use in Workflow**: Pass the logger to `WorkflowRunner` or individual workflow steps.

## Example

```csharp
// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

// Create a SerilogLogger instance
var logger = new SerilogLogger(Log.Logger);

// Define a workflow
var workflow = new WorkflowBuilder()
    .WithId(Guid.NewGuid())
    .WithName("Sample Workflow")
    .AddStep(new SampleStep(logger)) // Pass logger to the step
    .Build();

// Create a WorkflowRunner with the logger
var runner = new WorkflowRunner(logger: logger);

// Execute the workflow
await runner.ExecuteAsync(workflow, new WorkflowContext(workflow));
```

## License

This project is licensed under the MIT License.
