using SimpleWorkflowEngine.Steps;

namespace SimpleWorkflowEngine.Samples.Workflows;

public class ParallelWorkflow
{
    public static async Task Run()
    {
        var step1 = new ActionStep(async (context, cancellationToken) => Console.WriteLine("Executing Step 1..."));
        var step2 = new ActionStep(async (context, cancellationToken) => Console.WriteLine("Executing Step 2..."));
        var step3 = new ActionStep(async (context, cancellationToken) => Console.WriteLine("Executing Step 3..."));

        var foreachStep = new ForeachStep(maxDegreeOfParallelism: 3);
        foreachStep.AddStep(step1);
        foreachStep.AddStep(step2);
        foreachStep.AddStep(step3);

        var workflow = new WorkflowBuilder()
            .WithId(Guid.NewGuid())
            .WithName("Parallel Workflow Example")
            .AddStep(foreachStep)
            .Build();

        var runner = new WorkflowEngine([], new WorkflowSettings());

        await runner.ExecuteAsync(workflow, new WorkflowContext(workflow));
    }
}