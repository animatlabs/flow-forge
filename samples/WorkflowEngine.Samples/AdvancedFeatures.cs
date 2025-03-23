//using AnimatLabs.FlowForge.Core;
//using AnimatLabs.FlowForge.Middleware;
//using AnimatLabs.WorkflowEngine.Exceptions;
//using OpenTelemetry.Trace;
//using Serilog;
//using System;
//using System.Reflection.Metadata;
//using System.Threading;
//using System.Threading.Tasks;

namespace AdvancedFeatures;

public class Program
{
    public static async Task Main()
    {
        //        // Configure logging
        //        Log.Logger = new LoggerConfiguration()
        //            .WriteTo.Console()
        //            .CreateLogger();

        //        // Configure tracing
        //        var tracer = TracerProvider.Default.GetTracer("FlowForge");

        //        // Configure settings
        //        var settings = new ForgeSettings
        //        {
        //            MaxConcurrency = 20,
        //            CompensationRetries = 3
        //        };

        //        // Build workflow
        //        var flow = new FlowBuilder()
        //            .AddMiddleware(new LoggingMiddleware())
        //            .AddMiddleware(new TracingMiddleware(tracer))
        //            .AddMiddleware(new RetryMiddleware(3, ex => ex is not InvalidOperationException))
        //            .AddStep(new DatabaseStep(new SqlConnection("..."))
        //            .Build();

        //        // Execute workflow
        //        var forge = new FlowForge(settings, new Throttler(settings.MaxConcurrency))
        //        {
        //            StepExecuting = (s, e) =>
        //                Log.Information("Starting step {StepId}", e.Step.StepId),

        //            StepExecuted = (s, e) =>
        //                Log.Information("Completed step {StepId} in {Duration}ms", e.Step.StepId, e.Duration.TotalMilliseconds),

        //            CompensationFailed = (s, e) =>
        //                Log.Error(e.Exception, "Compensation failed for {StepId}", e.Step.StepId)
        //        };

        //        try
        //        {
        //            await forge.ExecuteAsync(
        //                flow,
        //                new FlowContext(),
        //                new CancellationTokenSource(TimeSpan.FromMinutes(5)).Token
        //            );

        //            Log.Information("Workflow completed successfully!");
        //        }
        //        catch (WorkflowException ex)
        //        {
        //            Log.Error(ex, "Workflow failed: {Message}", ex.Message);
        //        }
    }
}