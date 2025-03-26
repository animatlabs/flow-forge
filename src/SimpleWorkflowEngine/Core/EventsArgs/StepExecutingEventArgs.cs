using System;

namespace SimpleWorkflowEngine.Core.EventsArgs
{
    /// <summary>
    /// Event arguments for the StepExecuting event.
    /// </summary>
    public class StepExecutingEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the step that is being executed.
        /// </summary>
        public IWorkflowStep Step { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StepExecutingEventArgs"/> class.
        /// </summary>
        /// <param name="step">The step that is being executed.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="step"/> is null.</exception>
        public StepExecutingEventArgs(IWorkflowStep step)
        {
            Step = step ?? throw new ArgumentNullException(nameof(step));
        }
    }
}