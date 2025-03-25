using WorkflowEngine.Steps;

namespace WorkflowEngine.Samples.Workflows;

public class NestedWorkflow
{
    public static async Task Run()
    {
        // Define a nested workflow
        var nestedWorkflow = new WorkflowBuilder()
            .WithId(Guid.NewGuid())
            .WithName("Nested Workflow")
            .AddStep(new ActionStep(async (context, cancellationToken) => Console.WriteLine("Executing Nested Step 1...")))
            .AddStep(new ActionStep(async (context, cancellationToken) => Console.WriteLine("Executing Nested Step 2...")))
            .Build();

        // Create a runner
        var runner = new WorkflowEngine([], new WorkflowSettings());

        // Define a parent workflow with a NestedWorkflowStep
        var nestedWorkflowStep = new NestedWorkflowStep(nestedWorkflow, runner);
        var parentWorkflow = new WorkflowBuilder()
            .WithId(Guid.NewGuid())
            .WithName("Parent Workflow")
            .AddStep(new ActionStep(async (context, cancellationToken) => Console.WriteLine("Executing Parent Step 1...")))
            .AddStep(nestedWorkflowStep)
            .AddStep(new ActionStep(async (context, cancellationToken) => Console.WriteLine("Executing Parent Step 2...")))
            .Build();

        // Execute the parent workflow
        await runner.ExecuteAsync(parentWorkflow, new WorkflowContext(parentWorkflow));
    }
}