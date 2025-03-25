using WorkflowEngine.Samples.Workflows;

namespace WorkflowEngine.Samples;

public class Program
{
    public static async Task Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Select a workflow to run:");

            // Basic Workflows
            Console.WriteLine("1. Basic Workflow");
            Console.WriteLine("2. Conditional Workflow");
            Console.WriteLine("3. Parallel Workflow");
            Console.WriteLine("4. Custom Step Workflow");

            // Retry Mechanisms
            Console.WriteLine("5. Retry Workflow");
            Console.WriteLine("6. Retry Strategies Workflow");

            // Middleware and Event Handlers
            Console.WriteLine("7. Middleware Workflow");
            Console.WriteLine("8. Event Handlers Workflow");

            // Nested Workflows
            Console.WriteLine("9. Nested Workflow");

            // Exit
            Console.WriteLine("10. Exit");

            Console.Write("Enter your choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await BasicWorkflow.Run();
                    break;

                case "2":
                    await ConditionalWorkflow.Run();
                    break;

                case "3":
                    await ParallelWorkflow.Run();
                    break;

                case "4":
                    await CustomStepWorkflow.Run();
                    break;

                case "5":
                    await RetryWorkflow.Run();
                    break;

                case "6":
                    await RetryStrategiesWorkflow.Run();
                    break;

                case "7":
                    await MiddlewareWorkflow.Run();
                    break;

                case "8":
                    await EventHandlersWorkflow.Run();
                    break;

                case "9":
                    await NestedWorkflow.Run();
                    break;

                case "10":
                    Console.WriteLine("Exiting...");
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }
    }
}