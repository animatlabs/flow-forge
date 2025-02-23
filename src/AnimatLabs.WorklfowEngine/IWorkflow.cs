using System;
using System.Threading;
using System.Threading.Tasks;
using AnimatLabs.WorkflowEngine.Core.EventsArgs;
using AnimatLabs.WorkflowEngine.Exceptions;

namespace AnimatLabs.WorkflowEngine
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
        /// Gets the unique identifier for the workflow.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the collection of workflow steps.
        /// </summary>
        IWorkflowStep[] Steps { get; }

        /// <summary>
        /// Occurs when a step starts executing.
        /// </summary>
        event EventHandler<StepExecutingEventArgs> StepExecuting;

        /// <summary>
        /// Occurs when a step completes execution.
        /// </summary>
        event EventHandler<StepExecutedEventArgs> StepExecuted;

        /// <summary>
        /// Occurs when compensation for a step fails.
        /// </summary>
        event EventHandler<CompensationFailedEventArgs> CompensationFailed;
    }
}