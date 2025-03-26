using SimpleWorkflowEngine.Steps;

namespace SimpleWorkflowEngine.Samples.Workflows;

public class EventHandlersWorkflow
{
    public static async Task Run()
    {
        // Define steps
        var step1 = new ActionStep(async (context, cancellationToken) =>
        {
            Console.WriteLine("Step 1: Start workflow");
        });
        var step2 = new ActionStep(async (context, cancellationToken) =>
        {
            Console.WriteLine("Step 2: End workflow");
        });

        // Create workflow
        var workflow = new WorkflowBuilder()
            .WithId(Guid.NewGuid())
            .WithName("Workflow with Event Handlers")
            .AddStep(step1)
            .AddStep(step2)
            .Build();

        workflow.StepExecuting += (sender, args) => Console.WriteLine($"Executing step: {args.Step.Name}");
        workflow.StepExecuted += (sender, args) => Console.WriteLine($"Executed step: {args.Step.Name}");

        // Create runner
        var runner = new WorkflowRunner([], new WorkflowSettings());

        // Execute workflow
        await runner.ExecuteAsync(workflow, new WorkflowContext(workflow));
    }
}