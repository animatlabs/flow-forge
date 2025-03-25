using System;
using System.Collections.Generic;

namespace WorkflowEngine
{
    /// <summary>
    /// Provides a fluent API for building workflows.
    /// </summary>
    public class WorkflowBuilder
    {
        private Guid id;
        private string name;
        private readonly List<IWorkflowStep> steps = new List<IWorkflowStep>();

        /// <summary>
        /// Sets the unique identifier for the workflow.
        /// </summary>
        /// <param name="id">The unique identifier for the workflow.</param>
        /// <returns>The current instance of the <see cref="WorkflowBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="id"/> is default.</exception>
        public WorkflowBuilder WithId(Guid id)
        {
            if (id == default)
                throw new ArgumentNullException(nameof(id));

            this.id = id;
            return this;
        }

        /// <summary>
        /// Sets the name of the workflow.
        /// </summary>
        /// <param name="name">The name of the workflow.</param>
        /// <returns>The current instance of the <see cref="WorkflowBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="name"/> is null or empty.</exception>
        public WorkflowBuilder WithName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            this.name = name;
            return this;
        }

        /// <summary>
        /// Adds a workflow step to the builder.
        /// </summary>
        /// <param name="step">The workflow step to add.</param>
        /// <returns>The current instance of the <see cref="WorkflowBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="step"/> is null.</exception>
        public WorkflowBuilder AddStep(IWorkflowStep step)
        {
            if (step == null)
                throw new ArgumentNullException(nameof(step));

            steps.Add(step);
            return this;
        }

        /// <summary>
        /// Builds the workflow with the configured steps.
        /// </summary>
        /// <returns>A new instance of the <see cref="Workflow"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no steps have been added to the builder.</exception>
        public IWorkflow Build()
        {
            if (steps.Count == 0)
                throw new InvalidOperationException("No steps have been added to the workflow.");

            return new Workflow(id, name, steps.ToArray());
        }
    }
}