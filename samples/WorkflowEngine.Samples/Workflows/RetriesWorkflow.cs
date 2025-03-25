using WorkflowEngine.Middlewares;
using WorkflowEngine.Steps;

namespace WorkflowEngine.Samples.Workflows;

public class WorkflowWithRetries
{
    public static async Task Run()
    {
        // Define a step that fails
        var failingStep = new ActionStep(async (context, cancellationToken) =>
        {
            Console.WriteLine("Executing FailingStep...");
            throw new Exception("Simulated failure");
        });

        // Create a workflow
        var workflow = new WorkflowBuilder()
            .WithId(Guid.NewGuid())
            .WithName("Retry Workflow")
            .AddStep(failingStep)
            .Build();

        // Define retry middleware
        var retryMiddleware = new RetryMiddleware(maxRetries: 3, delay: TimeSpan.FromSeconds(1));

        // Create a runner
        var runner = new WorkflowEngine([retryMiddleware], new WorkflowSettings());

        // Execute the workflow
        try
        {
            await runner.ExecuteAsync(workflow, new WorkflowContext(workflow));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Workflow failed after retries: {ex.Message}");
        }
    }
}