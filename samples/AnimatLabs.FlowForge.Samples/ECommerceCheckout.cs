//using AnimatLabs.FlowForge.Middleware;
//using AnimatLabs.WorkflowEngine;
//using System;
//using System.Threading;
//using System.Threading.Tasks;

//namespace ECommerceCheckout;

//class Program
//{
//    static async Task Main()
//    {
//        var settings = new WorkflowSettings(
//            autoCompensate: true,
//            continueOnCompensationFailure: true,
//            defaultStepTimeout: TimeSpan.FromSeconds(30),
//            maxConcurrentWorkflows: 20
//            );

//        var logger = new Logger(msg => Console.WriteLine($"[{DateTime.UtcNow}] {msg}"));
//        var forge = new FlowForge(settings, logger)
//            .AddMiddleware(new LoggingMiddleware(logger))
//            .AddMiddleware(new RetryMiddleware(3, TimeSpan.FromSeconds(1)));

//        var flow = new WorkflowBuilder()
//            .AddStep(new ProcessPaymentStep())
//            .AddStep(new ReserveInventoryStep())
//            .Build();

//        forge.StepExecuting += (sender, e) =>
//            logger.LogInformation($"Starting step: {e.Step.StepId}");

//        forge.StepExecuted += (sender, e) =>
//            logger.LogInformation($"Step {e.Step.StepId} completed in {e.Duration.TotalMilliseconds}ms");

//        forge.CompensationFailed += (sender, e) =>
//            logger.LogError($"Compensation failed for step {e.Step.StepId}: {e.Exception.Message}");

//        await forge.ExecuteAsync(new FlowContext());
//    }
//}

//public class ProcessPaymentStep : AnimatLabs.WorkflowEngine.WorkflowStep
//{
//    protected override Task ExecuteCoreAsync(WorkflowContext context, CancellationToken ct)
//    {
//        Console.WriteLine("Processing payment...");
//        return Task.CompletedTask;
//    }

//    protected override Task CompensateCoreAsync(WorkflowContext context, CancellationToken ct)
//    {
//        Console.WriteLine("Refunding payment...");
//        return Task.CompletedTask;
//    }
//}

//public class ReserveInventoryStep : AnimatLabs.WorkflowEngine.WorkflowStep
//{
//    protected override Task ExecuteCoreAsync(WorkflowContext context, CancellationToken ct)
//    {
//        Console.WriteLine("Reserving inventory...");
//        return Task.CompletedTask;
//    }

//    protected override Task CompensateCoreAsync(WorkflowContext context, CancellationToken ct)
//    {
//        Console.WriteLine("Releasing inventory...");
//        return Task.CompletedTask;
//    }
//}