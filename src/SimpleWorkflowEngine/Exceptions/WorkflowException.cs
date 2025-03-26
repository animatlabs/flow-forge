using System;

namespace SimpleWorkflowEngine.Exceptions
{
    /// <summary>
    /// Represents an exception that occurs during the execution of a workflow
    /// </summary>
    public class WorkflowException : Exception
    {
        /// <summary>
        /// Create an instance of the <see cref="WorkflowException"/> class.
        /// </summary>
        public WorkflowException()
        {
        }

        /// <summary>
        /// Create an instance of the <see cref="WorkflowException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public WorkflowException(string message) : base(message)
        {
        }

        /// <summary>
        /// Create an instance of the <see cref="WorkflowException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public WorkflowException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}