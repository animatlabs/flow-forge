using System;

namespace AnimatLabs.WorkflowEngine.Core.EventsArgs
{
    /// <summary>
    /// Event arguments for the StepExecuted event.
    /// </summary>
    public class StepExecutedEventArgs : EventArgs
    {
        /// <summary>
        /// The step that completed execution.
        /// </summary>
        public IWorkflowStep Step { get; }

        /// <summary>
        /// The duration of the step's execution.
        /// </summary>
        public TimeSpan Duration { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StepExecutedEventArgs"/> class.
        /// </summary>
        /// <param name="step">The step that completed execution.</param>
        /// <param name="duration">The duration of the step's execution.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="step"/> is null.</exception>
        public StepExecutedEventArgs(IWorkflowStep step, TimeSpan duration)
        {
            Step = step ?? throw new ArgumentNullException(nameof(step));
            Duration = duration;
        }
    }
}