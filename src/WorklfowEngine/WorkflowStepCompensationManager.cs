using System;
using System.Collections.Concurrent;

namespace WorkflowEngine
{
    /// <summary>
    /// Manages the compensation of workflow steps.
    /// </summary>
    internal sealed class WorkflowStepCompensationManager : IWorkflowStepCompensationManager
    {
        private readonly ConcurrentDictionary<string, byte> compensatedWorkflowSteps;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowStepCompensationManager"/> class.
        /// </summary>
        public WorkflowStepCompensationManager()
        {
            compensatedWorkflowSteps = new ConcurrentDictionary<string, byte>();
        }

        /// <inheritdoc />
        public void MarkCompensated(Guid workflowId, Guid stepId) => compensatedWorkflowSteps.TryAdd($"{workflowId}:{stepId}", 0);

        /// <inheritdoc />
        public bool IsCompensated(Guid workflowId, Guid stepId) => compensatedWorkflowSteps.ContainsKey($"{workflowId}:{stepId}");
    }
}