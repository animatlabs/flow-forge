using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WorkflowEngine.Steps
{
    /// <summary>
    /// Represents a workflow step that executes a nested workflow.
    /// </summary>
    public sealed class NestedWorkflowStep : WorkflowStep
    {
        private readonly IWorkflow nestedWorkflow;
        private readonly WorkflowEngine workflowRunner;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="NestedWorkflowStep"/> class.
        /// </summary>
        /// <param name="nestedWorkflow">The nested workflow to execute.</param>
        /// <param name="workflowRunner">The workflow runner to use for execution.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="nestedWorkflow"/> or <paramref name="workflowRunner"/> is null.
        /// </exception>
        public NestedWorkflowStep(IWorkflow nestedWorkflow, WorkflowEngine workflowRunner)

        {
            this.nestedWorkflow = nestedWorkflow ?? throw new ArgumentNullException(nameof(nestedWorkflow));
            this.workflowRunner = workflowRunner ?? throw new ArgumentNullException(nameof(workflowRunner));
        }

        /// <inheritdoc/>
        protected override async Task ExecuteCoreAsync(IWorkflowContext context, CancellationToken cancellationToken)
        {
            using (_ = logger.BeginScope(new Dictionary<string, object> {
                    { "ParentWorkflowId", context.Workflow.WorkflowId },
                    { "ParentWorkflowName", context.Workflow.Name },
                    { "ParentStepName", Name },
                    { "ParentStepId", StepId },
                    { "ParentStepName", Name }
                }))
            {
                await workflowRunner.ExecuteAsync(nestedWorkflow, context, cancellationToken).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        protected override async Task CompensateCoreAsync(IWorkflowContext context, CancellationToken cancellationToken) => await Task.CompletedTask.ConfigureAwait(false);

        /// <inheritdoc/>
        protected override void DisposeCore()
        {
            nestedWorkflow.Dispose();
        }
    }
}