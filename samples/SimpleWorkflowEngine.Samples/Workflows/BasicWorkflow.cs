using SimpleWorkflowEngine.Steps;

namespace SimpleWorkflowEngine.Samples.Workflows;

public class BasicWorkflow
{
    public static async Task Run()
    {
        var step1 = new ActionStep(async (context, cancellationToken) => Console.WriteLine("Step 1: Start workflow"));
        var step2 = new ActionStep(async (context, cancellationToken) => Console.WriteLine("Step 2: End workflow"));

        var workflow = new WorkflowBuilder()
            .WithId(Guid.NewGuid())
            .WithName("Basic Workflow")
            .AddStep(step1)
            .AddStep(step2)
            .Build();

        var runner = new WorkflowEngine([], new WorkflowSettings());
        await runner.ExecuteAsync(workflow, new WorkflowContext(workflow));
    }
}