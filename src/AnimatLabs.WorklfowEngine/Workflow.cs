using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using AnimatLabs.WorkflowEngine.Core;
using AnimatLabs.WorkflowEngine.Core.EventsArgs;
using AnimatLabs.WorkflowEngine.Exceptions;

namespace AnimatLabs.WorkflowEngine
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
            this.WorkflowId = workflowId;
            this.Name = workflowName;
            this.Steps = steps;
        }

        /// <inheritdoc />
        public Guid WorkflowId { get; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public IWorkflowStep[] Steps { get; }
        
        /// <inheritdoc />
        public event EventHandler<StepExecutingEventArgs> StepExecuting;

        /// <inheritdoc />
        public event EventHandler<StepExecutedEventArgs> StepExecuted;

        /// <inheritdoc />
        public event EventHandler<CompensationFailedEventArgs> CompensationFailed;

        public void Dispose()
        {
            foreach (var step in Steps)
            {
                step.Dispose();
            }
        }
    }
}