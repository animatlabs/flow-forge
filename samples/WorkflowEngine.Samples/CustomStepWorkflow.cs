using Serilog;
using WorkflowEngine.Logger.Serilog;

namespace WorkflowEngine.Samples;

public class CustomStepWorkflow
{
    public static async Task Run()
    {
        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        var logger = new SerilogLogger(Log.Logger);

        // Define a custom step
        var customStep = new CustomStep("Custom Step Message", logger);

        // Create a workflow
        var workflow = new WorkflowBuilder()
            .WithId(Guid.NewGuid())
            .WithName("Custom Step Workflow")
            .AddStep(customStep)
            .Build();

        // Create a runner
        var runner = new WorkflowEngine([], new WorkflowSettings());

        // Execute the workflow
        await runner.ExecuteAsync(workflow, new WorkflowContext(workflow));
    }
}

public class CustomStep(string message, Core.ILogger logger = null) : WorkflowStep(logger)
{
    private readonly string _message = message;

    protected override async Task ExecuteCoreAsync(IWorkflowContext context, CancellationToken cancellationToken)
    {
        logger.LogInformation("Executing custom step with message: {Message}", _message);
        await Task.CompletedTask;
    }

    protected override Task CompensateCoreAsync(IWorkflowContext context, CancellationToken cancellationToken)
    {
        logger.LogInformation("Compensating custom step with message: {Message}", _message);
        return Task.CompletedTask;
    }

    protected override void DisposeCore()
    {
        // do nothing
    }
}
