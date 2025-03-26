# Workflow Engine

A robust, extensible, and dependency-free workflow framework for .NET. Designed with modularity in mind, this library provides a flexible API for defining, managing, and executing workflows. It is built on `.NET Standard 2.0`, ensuring compatibility with `.NET Framework`, `.NET Core`, and newer versions of `.NET`.

## Key Principles

- **Dependency-Free Core**: The core `SimpleWorkflowEngine` project has no external dependencies, making it lightweight and suitable for environments with strict dependency policies.
- **Modular Design**: Additional features like logging, tracing, and retry strategies are implemented as separate projects, allowing you to include only what you need.
- **Cross-Platform Compatibility**: Built on `.NET Standard 2.0`, ensuring it works seamlessly across different .NET runtimes.

## Features

- **Fluent API**: Build workflows with a clean and intuitive syntax.
- **Compensation**: Automatic rollback on failures.
- **Middleware Support**: Add custom logic, logging, retries, and timeouts.
- **Parallel Execution**: Run steps concurrently with throttling support.
- **Retry Strategies**: Built-in strategies like exponential backoff, fixed intervals, and random intervals.
- **Cancellation Support**: First-class support for cancellation tokens.
- **OpenTelemetry Integration**: Optional middleware for distributed tracing.
- **Serilog Integration**: Optional logging support using Serilog.
- **Extensibility**: Easily extend the framework to meet custom requirements.

---

## Getting Started

### 1. Install the Workflow Engine

Clone the repository and include the `SimpleWorkflowEngine` project in your solution. Since the core library has no dependencies, you can use it directly without worrying about external packages.

Alternatively, you can include additional projects like `SimpleWorkflowEngine.Logger.Serilog` or `SimpleWorkflowEngine.Middlewares.OpenTelemetry` based on your requirements.

### 2. Define Workflow Steps

Create custom workflow steps by inheriting from `WorkflowStep`. Each step must implement the `ExecuteCoreAsync` method and optionally the `CompensateCoreAsync` method for rollback logic.

```csharp
using SimpleWorkflowEngine;

public class SampleStep : WorkflowStep
{
    protected override Task ExecuteCoreAsync(IWorkflowContext context, CancellationToken ct)
    {
        Console.WriteLine("Executing SampleStep...");
        return Task.CompletedTask;
    }

    protected override Task CompensateCoreAsync(IWorkflowContext context, CancellationToken ct)
    {
        Console.WriteLine("Compensating SampleStep...");
        return Task.CompletedTask;
    }
}
```

### 3. Build a Workflow

Use the `WorkflowBuilder` to define the workflow steps. The builder ensures a fluent and intuitive API for constructing workflows.

```csharp
using SimpleWorkflowEngine;

var workflow = new WorkflowBuilder()
    .WithId(Guid.NewGuid())
    .WithName("Sample Workflow")
    .AddStep(new SampleStep())
    .Build();
```

### 4. Execute the Workflow

Use the `WorkflowRunner` to execute the workflow. You can optionally include middleware for logging, retries, or other custom logic.

```csharp
using SimpleWorkflowEngine;
using SimpleWorkflowEngine.Middlewares;

var runner = new WorkflowRunner(new List<IWorkflowStepMiddleware>
{
    new LoggingMiddleware(new SerilogLogger(Log.Logger)),
    new RetryMiddleware(3, TimeSpan.FromSeconds(1))
});

var context = new WorkflowContext(workflow);

await runner.ExecuteAsync(workflow, context);
```

---

## Advanced Features

### Middleware

Middleware allows you to intercept and extend the behavior of workflow steps. Examples include logging, retries, and timeouts.

```csharp
var middleware = new List<IWorkflowStepMiddleware>
{
    new LoggingMiddleware(new SerilogLogger(Log.Logger)),
    new TimeoutMiddleware(TimeSpan.FromSeconds(30)),
    new RetryMiddleware(3, TimeSpan.FromSeconds(1))
};
```

### Parallel Execution

Use the `ForeachStep` to execute steps concurrently. You can control the degree of parallelism using the `maxDegreeOfParallelism` parameter.

```csharp
var parallelStep = new ForeachStep(maxDegreeOfParallelism: 5);
parallelStep.AddStep(new SampleStep());
parallelStep.AddStep(new AnotherStep());
```

### Retry Strategies

The core library includes built-in retry strategies like `ExponentialBackoffRetryStrategy`, `FixedIntervalRetryStrategy`, and `RandomIntervalRetryStrategy`. You can also implement your own by inheriting from `IRetryStrategy`.

```csharp
var retryStrategy = new ExponentialBackoffRetryStrategy(
    initialInterval: TimeSpan.FromMilliseconds(100),
    maxInterval: TimeSpan.FromSeconds(5),
    maxRetryCount: 3
);
```

### OpenTelemetry Integration

For distributed tracing, include the `SimpleWorkflowEngine.Middlewares.OpenTelemetry` project and use the `TracingMiddleware`.

```csharp
var tracer = TracerProvider.Default.GetTracer("WorkflowEngine");
var middleware = new TracingMiddleware(tracer);
```

### Serilog Integration

For logging, include the `SimpleWorkflowEngine.Logger.Serilog` project and use the `SerilogLogger`.

```csharp
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var logger = new SerilogLogger(Log.Logger);
```

---

## Common Use Cases

The Workflow Engine is versatile and can be used in various scenarios, such as:

1. **E-Commerce Checkout Process**:
   - Steps: Validate cart, process payment, reserve inventory, and send confirmation email.
   - Compensation: Roll back payment and release inventory if any step fails.

2. **Data Processing Pipelines**:
   - Steps: Fetch data, transform data, and save results to a database.
   - Parallel Execution: Process multiple data chunks concurrently.

3. **Microservices Orchestration**:
   - Steps: Call multiple microservices in sequence or parallel.
   - Middleware: Add retries and logging for each service call.

4. **Approval Workflows**:
   - Steps: Submit request, notify approvers, and finalize approval.
   - Compensation: Revert changes if the request is rejected.

---

## FAQ

### Why is the core library dependency-free?
The core library is designed to be lightweight and adaptable to environments with strict dependency policies. Optional features like logging and tracing are implemented as separate projects to give you full control over dependencies.

### Can I use this library in a legacy .NET Framework project?
Yes! The library is built on `.NET Standard 2.0`, ensuring compatibility with `.NET Framework 4.6.1` and later.

### How do I add custom middleware?
You can create a class that implements `IWorkflowStepMiddleware` and add it to the `WorkflowRunner` during initialization. For example:

```csharp
public class CustomMiddleware : IWorkflowStepMiddleware
{
    public async Task InvokeAsync(IWorkflowStep step, IWorkflowContext context, Func<Task> next, CancellationToken ct)
    {
        Console.WriteLine($"Before executing step: {step.Name}");
        await next();
        Console.WriteLine($"After executing step: {step.Name}");
    }
}
```

---

## Best Practices

- **Keep Steps Small and Focused**: Each step should perform a single, well-defined task. This makes workflows easier to debug and maintain.
- **Use Middleware for Cross-Cutting Concerns**: Add logging, retries, and timeouts as middleware instead of embedding them in workflow steps.
- **Leverage Compensation**: Always implement `CompensateCoreAsync` for steps that modify external systems to ensure proper rollback on failures.
- **Test Workflows Thoroughly**: Use unit tests to validate workflows, especially for complex scenarios with compensation and parallel execution.
- **Avoid Overloading Parallel Steps**: When using `ForeachStep`, set a reasonable `maxDegreeOfParallelism` to avoid overwhelming system resources.

---

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request.

---

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.