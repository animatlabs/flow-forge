using WorkflowEngine.Steps;

namespace WorkflowEngine.Samples;

public class ConditionalWorkflow
{
    public static async Task Run()
    {
        // Define a conditional step
        var conditionalStep = new ConditionalStep(
            context => DateTime.Now.Second % 2 == 0,
            new ActionStep(async (context, cancellationToken) => Console.WriteLine("Condition is true!")),
            new ActionStep(async (context, cancellationToken) => Console.WriteLine("Condition is false!"))
        );

        // Create a workflow
        var workflow = new WorkflowBuilder()
            .WithId(Guid.NewGuid())
            .WithName("Conditional Workflow Example")
            .AddStep(conditionalStep)
            .Build();

        // Create a runner
        var runner = new WorkflowEngine([], new WorkflowSettings());

        // Execute the workflow
        await runner.ExecuteAsync(workflow, new WorkflowContext(workflow));
    }
}