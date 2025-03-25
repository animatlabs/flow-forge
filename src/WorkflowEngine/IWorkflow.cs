using System;
using WorkflowEngine.Core.EventsArgs;

namespace WorkflowEngine
{
    /// <summary>
    /// Interface for Workflow to improve testability
    /// </summary>
    public interface IWorkflow : IDisposable
    {
        /// <summary>
        /// Gets the unique identifier for the workflow.
        /// </summary>
        Guid WorkflowId { get; }

        /// <summary>
        /// Gets the name of the workflow.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the collection of workflow steps.
        /// </summary>
        IWorkflowStep[] Steps { get; }

        /// <summary>
        /// Occurs when a step is about to be executed.
        /// </summary>
        event EventHandler<StepExecutingEventArgs> StepExecuting;

        /// <summary>
        /// Occurs when a step has been executed.
        /// </summary>
        event EventHandler<StepExecutedEventArgs> StepExecuted;

        /// <summary>
        /// Occurs when a step compensation has failed.
        /// </summary>
        event EventHandler<CompensationFailedEventArgs> CompensationFailed;
    }
}