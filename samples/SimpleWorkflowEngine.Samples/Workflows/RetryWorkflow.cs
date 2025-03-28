using Serilog;
using SimpleWorkflowEngine.Core.RetryStrategy;
using SimpleWorkflowEngine.Logger.Serilog;
using SimpleWorkflowEngine.Steps;

namespace SimpleWorkflowEngine.Samples.Workflows;

public class RetryWorkflow
{
    public static async Task Run()
    {
        var failingStep = new ActionStep(async (context, cancellationToken) =>
        {
            Console.WriteLine("Executing FailingStep...");
            throw new Exception("Simulated failure");
        });

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        var logger = new SerilogLogger(Log.Logger);

        var retryStep = new RetryStep(
            failingStep,
            maxRetries: 3,
            retryStrategy: new FixedIntervalRetryStrategy(TimeSpan.FromSeconds(1), 3),
            logger: logger
        );

        var workflow = new WorkflowBuilder()
            .WithId(Guid.NewGuid())
            .WithName("Retry Workflow Example")
            .AddStep(retryStep)
            .Build();

        var runner = new WorkflowRunner([], new WorkflowSettings());

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