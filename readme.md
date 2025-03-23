
# Workflow Engine

A robust workflow framework for .NET with compensation, middleware, and parallel execution. This library has no external dependencies, making it lightweight and easy to integrate into any .NET project.

## Features
- **Fluent API** - Build workflows with a clean syntax
- **Compensation** - Automatic rollback on failures
- **Middleware** - Add logging, retries, timeouts
- **Thread-Safe** - Safe for concurrent workflows
- **Cancellation** - First-class cancellation support

## Quick Start

1. Define steps:2. Build workflow:3. Execute:## Advanced Usage

### Parallel Steps
Use `ParallelStep` to run steps concurrently:### Middleware
Add logging, retries, or custom logic:### Events
Monitor step execution and compensation:## Samples

See [SAMPLES.md](/docs/samples.md) for advanced examples.

### **How to Use This Code**
1. Create a new .NET project
2. Copy the folder structure and files
3. Compile with:### Run samples:## No External Dependencies

This workflow library is designed to be lightweight and has no external dependencies. This ensures that it can be easily integrated into any .NET project without worrying about compatibility issues or bloat.

## License

This project is licensed under the MIT License - see the [LICENSE.md](/LICENSE.md) file for details.
# Workflow Engine

A robust workflow framework for .NET with compensation, middleware, and parallel execution.

## Features
- **Fluent API** - Build workflows with a clean syntax
- **Compensation** - Automatic rollback on failures
- **Middleware** - Add logging, retries, timeouts
- **Thread-Safe** - Safe for concurrent workflows
- **Cancellation** - First-class cancellation support

## Quick Start

1. Define steps:
```csharp
public class SampleStep : Step
{
    protected override Task ExecuteCoreAsync(FlowContext context, CancellationToken ct)
    {
        context.Set("Data", "Processed");
        return Task.CompletedTask;
    }
}
```

2. Build workflow:

```csharp
var flow = new FlowBuilder().AddStep(new SampleStep()).Build();
	await new FlowForge().ExecuteAsync(new FlowContext());
```

3. Execute:

```csharp
await new FlowForge().ExecuteAsync(new FlowContext());
```

## Advanced Usage

- Parallel Steps: Use ParallelStep to run steps concurrently
- Middleware: Add logging, retries, or custom logic
- Events: Monitor step execution and compensation

## Samples

See (SAMPLES.md)[/docs/samples.md] for advanced examples.


### **How to Use This Code**
1. Create a new .NET project
2. Copy the folder structure and files
3. Compile with:

```bash
dotnet build
```

### Run samples:

```bash
dotnet run --project samples/ECommerceCheckout
```

