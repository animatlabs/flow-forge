using System;

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

            // Retry Mechanisms
            Console.WriteLine("2. Retry Workflow");
            Console.WriteLine("3. Retry Strategies Workflow");

            // Middleware and Customization
            Console.WriteLine("4. Middleware Workflow");
            Console.WriteLine("5. Custom Step Workflow");

            // Event Handling
            Console.WriteLine("6. Event Handlers Workflow");

            // Workflow Structures
            Console.WriteLine("7. Action Step Workflow");
            Console.WriteLine("8. Parallel Workflow");
            Console.WriteLine("9. Nested Workflow");
            Console.WriteLine("10. Conditional Workflow");

            // Exit
            Console.WriteLine("11. Exit");

            Console.Write("Enter your choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await BasicWorkflow.Run();
                    break;
                case "2":
                    await RetryWorkflow.Run();
                    break;
                case "3":
                    await RetryStrategiesWorkflow.Run();
                    break;
                case "4":
                    await MiddlewareWorkflow.Run();
                    break;
                case "5":
                    await CustomStepWorkflow.Run();
                    break;
                case "6":
                    await EventHandlersWorkflow.Run();
                    break;
                case "7":
                    await ActionStepWorkflow.Run();
                    break;
                case "8":
                    await ParallelWorkflow.Run();
                    break;
                case "9":
                    await NestedWorkflow.Run();
                    break;
                case "10":
                    await ConditionalWorkflow.Run();
                    break;
                case "11":
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
