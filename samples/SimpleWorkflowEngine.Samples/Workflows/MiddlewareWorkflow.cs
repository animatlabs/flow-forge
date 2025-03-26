using SimpleWorkflowEngine.Steps;

namespace SimpleWorkflowEngine.Samples.Workflows;

public class MiddlewareWorkflow
{
    public static async Task Run()
    {
        // Define steps
        var step1 = new ActionStep(async (context, cancellationToken) => Console.WriteLine("Executing Step 1..."));
        var step2 = new ActionStep(async (context, cancellationToken) => Console.WriteLine("Executing Step 2..."));

        // Create a workflow
        var workflow = new WorkflowBuilder()
            .WithId(Guid.NewGuid())
            .WithName("Workflow with Middleware")
            .AddStep(step1)
            .AddStep(step2)
            .Build();

        // Define middleware
        var loggingMiddleware = new LoggingMiddleware();

        // Create a runner
        var runner = new WorkflowEngine([loggingMiddleware], new WorkflowSettings());

        // Execute the workflow
        await runner.ExecuteAsync(workflow, new WorkflowContext(workflow));
    }
}

public class LoggingMiddleware : IWorkflowStepMiddleware
{
    public async Task ExecuteAsync(IWorkflowStep step, IWorkflowContext context, Func<Task> next, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Before executing step: {step.Name}");
        await next();
        Console.WriteLine($"After executing step: {step.Name}");
    }
}