using System;

namespace WorkflowEngine
{
    /// <summary>
    /// Executes workflows with compensation, middleware, and throttling
    /// </summary>
    internal sealed class Workflow : IWorkflow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Workflow"/> class.
        /// </summary>
        /// <param name="workflowId">The unique identifier for the workflow.</param>
        /// <param name="workflowName"></param>
        /// <param name="steps">The steps to be executed in the workflow.</param>
        internal Workflow(
            Guid workflowId,
            string workflowName,
            IWorkflowStep[] steps
        )
        {
            WorkflowId = workflowId;
            Name = workflowName;
            Steps = steps;
        }

        /// <inheritdoc />
        public Guid WorkflowId { get; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public IWorkflowStep[] Steps { get; }

        public void Dispose()
        {
            foreach (var step in Steps)
            {
                step.Dispose();
            }
        }
    }
}