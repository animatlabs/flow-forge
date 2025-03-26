using System;

namespace SimpleWorkflowEngine.Core.EventsArgs
{
    /// <summary>
    /// Event arguments for the CompensationFailed event.
    /// </summary>
    public class CompensationFailedEventArgs : EventArgs
    {
        /// <summary>
        /// The step that failed compensation.
        /// </summary>
        public IWorkflowStep Step { get; }

        /// <summary>
        /// The workflow context in which the failure occurred.
        /// </summary>
        public IWorkflowContext WorkflowContext { get; }

        /// <summary>
        /// The exception that caused the failure.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompensationFailedEventArgs"/> class.
        /// </summary>
        /// <param name="step">The step that failed compensation.</param>
        /// <param name="exception">The exception that caused the failure.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="step"/> or <paramref name="exception"/> is null.</exception>
        public CompensationFailedEventArgs(IWorkflowStep step, Exception exception)
        {
            Step = step ?? throw new ArgumentNullException(nameof(step));
            Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        }
    }
}