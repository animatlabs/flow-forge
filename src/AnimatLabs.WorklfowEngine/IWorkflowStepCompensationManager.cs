using System;

namespace AnimatLabs.WorkflowEngine
{
    /// <summary>
    /// Interface for managing workflow step compensations.
    /// </summary>
    public interface IWorkflowStepCompensationManager
    {
        /// <summary>
        /// Marks the specified step as compensated.
        /// </summary>
        /// <param name="workflowId">The identifier of the workflow.</param>
        /// <param name="stepId">The identifier of the step to mark as compensated.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="workflowId"/> or <paramref name="stepId"/> is null.</exception>
        void MarkCompensated(Guid workflowId, Guid stepId);

        /// <summary>
        /// Checks if the specified step has been compensated.
        /// </summary>
        /// <param name="workflowId">The identifier of the workflow.</param>
        /// <param name="stepId">The identifier of the step to check.</param>
        /// <returns>True if the step has been compensated; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="workflowId"/> or <paramref name="stepId"/> is null.</exception>
        bool IsCompensated(Guid workflowId, Guid stepId);
    }
}