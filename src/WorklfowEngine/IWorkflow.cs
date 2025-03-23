using System;

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
        /// Gets the unique identifier for the workflow.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the collection of workflow steps.
        /// </summary>
        IWorkflowStep[] Steps { get; }
    }
}