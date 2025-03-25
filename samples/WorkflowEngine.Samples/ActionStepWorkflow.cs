using WorkflowEngine.Steps;

namespace WorkflowEngine.Samples;

public class ActionStepWorkflow
{
    public static async Task Run()
    {
        // Define an ActionStep
        var actionStep = new ActionStep(async (context, cancellationToken) =>
        {
            Console.WriteLine("Executing ActionStep...");
        });

        // Create a workflow
        var workflow = new WorkflowBuilder()
            .WithId(Guid.NewGuid())
            .WithName("Workflow with Action Step")
            .AddStep(actionStep)
            .Build();

        // Create a runner
        var runner = new WorkflowEngine([], new WorkflowSettings());

        // Execute the workflow
        await runner.ExecuteAsync(workflow, new WorkflowContext(workflow));
    }
}