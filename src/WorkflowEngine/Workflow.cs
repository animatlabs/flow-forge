using System;
using WorkflowEngine.Core.EventsArgs;

namespace WorkflowEngine
{
    /// <summary>
    /// Represents a workflow that consists of multiple steps.
    /// </summary>
    internal sealed class Workflow : IWorkflow
    {
        private bool isDisposed; // Safeguard against cyclic disposal

        /// <summary>
        /// Initializes a new instance of the <see cref="Workflow"/> class.
        /// </summary>
        /// <param name="workflowId">The unique identifier for the workflow.</param>
        /// <param name="workflowName">The name of the workflow.</param>
        /// <param name="steps">The steps to be executed in the workflow.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="steps"/> is null.</exception>
        internal Workflow(
            Guid workflowId,
            string workflowName,
            IWorkflowStep[] steps
        )
        {
            WorkflowId = workflowId;
            Name = workflowName ?? throw new ArgumentNullException(nameof(workflowName));
            Steps = steps ?? throw new ArgumentNullException(nameof(steps));
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

        /// <inheritdoc />
        public void Dispose()
        {
            if (isDisposed) return; // Prevent multiple or cyclic disposal
            isDisposed = true;

            foreach (var step in Steps)
            {
                step.Dispose();
            }
        }

        /// <summary>
        /// Raises the <see cref="StepExecuting"/> event.
        /// </summary>
        /// <param name="stepExecutingEventArgs">The event data.</param>
        internal void OnStepExecuting(StepExecutingEventArgs stepExecutingEventArgs)
        {
            // Check if there are any subscribers
            StepExecuting?.Invoke(this, stepExecutingEventArgs);
        }

        /// <summary>
        /// Raises the <see cref="StepExecuted"/> event.
        /// </summary>
        /// <param name="stepExecutedEventArgs">The event data.</param>
        internal void OnStepExecuted(StepExecutedEventArgs stepExecutedEventArgs)
        {
            // Check if there are any subscribers
            StepExecuted?.Invoke(this, stepExecutedEventArgs);
        }

        /// <summary>
        /// Raises the <see cref="CompensationFailed"/> event.
        /// </summary>
        /// <param name="compensationFailedEventArgs">The event data.</param>
        internal void OnCompensationFailed(CompensationFailedEventArgs compensationFailedEventArgs)
        {
            // Check if there are any subscribers
            CompensationFailed?.Invoke(this, compensationFailedEventArgs);
        }
    }
}