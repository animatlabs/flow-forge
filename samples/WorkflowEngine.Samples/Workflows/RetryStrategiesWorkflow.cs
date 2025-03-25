using Serilog;
using WorkflowEngine.Core.RetryStrategy;
using WorkflowEngine.Logger.Serilog;
using WorkflowEngine.Steps;

namespace WorkflowEngine.Samples.Workflows;

public class RetryStrategiesWorkflow
{
    public static async Task Run()
    {
        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        var logger = new SerilogLogger(Log.Logger);

        // Define steps with different retry strategies
        var fixedRetryStep = new RetryStep(
            new ActionStep(async (context, cancellationToken) =>
            {
                Console.WriteLine("Executing FixedRetryStep...");
                throw new Exception("Simulated failure");
            }),
            maxRetries: 3,
            retryStrategy: new FixedIntervalRetryStrategy(TimeSpan.FromSeconds(1), 3),
            logger: logger
        );

        var exponentialRetryStep = new RetryStep(
            new ActionStep(async (context, cancellationToken) =>
            {
                Console.WriteLine("Executing ExponentialRetryStep...");
                throw new Exception("Simulated failure");
            }),
            maxRetries: 3,
            retryStrategy: new ExponentialBackoffRetryStrategy(TimeSpan.FromMilliseconds(500), TimeSpan.FromSeconds(5), 3),
            logger: logger
        );

        var randomRetryStep = new RetryStep(
            new ActionStep(async (context, cancellationToken) =>
            {
                Console.WriteLine("Executing RandomRetryStep...");
                throw new Exception("Simulated failure");
            }),
            maxRetries: 3,
            retryStrategy: new RandomIntervalRetryStrategy(TimeSpan.FromMilliseconds(500), TimeSpan.FromSeconds(2)),
            logger: logger
        );

        // Create a workflow with all retry steps
        var workflow = new WorkflowBuilder()
            .WithId(Guid.NewGuid())
            .WithName("Retry Strategies Workflow")
            .AddStep(fixedRetryStep)
            .AddStep(exponentialRetryStep)
            .AddStep(randomRetryStep)
            .Build();

        // Create a runner
        var runner = new WorkflowEngine([], new WorkflowSettings());

        // Execute the workflow
        try
        {
            await runner.ExecuteAsync(workflow, new WorkflowContext(workflow));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Workflow failed: {ex.Message}");
        }
    }
}