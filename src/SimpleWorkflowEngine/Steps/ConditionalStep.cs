using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SimpleWorkflowEngine.Core;

namespace SimpleWorkflowEngine.Steps
{
    /// <summary>
    /// Represents a conditional workflow step that executes one of two steps based on a condition.
    /// </summary>
    public sealed class ConditionalStep : WorkflowStep
    {
        private readonly Func<IWorkflowContext, bool> condition;
        private readonly IWorkflowStep trueStep;
        private readonly IWorkflowStep falseStep;

        private IWorkflowStep step;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalStep"/> class.
        /// </summary>
        /// <param name="condition">
        /// The condition to evaluate. If it returns <c>true</c>, the <paramref name="trueStep"/> is executed;
        /// otherwise, the <paramref name="falseStep"/> is executed.
        /// </param>
        /// <param name="trueStep">The step to execute if the condition is true. Cannot be null.</param>
        /// <param name="falseStep">The step to execute if the condition is false. Cannot be null.</param>
        /// <param name="logger">The logger instance to use for logging (optional).</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="condition"/>, <paramref name="trueStep"/>, or <paramref name="falseStep"/> is null.
        /// </exception>
        public ConditionalStep(Func<IWorkflowContext, bool> condition, IWorkflowStep trueStep, IWorkflowStep falseStep, ILogger logger = null)
            : base(logger)
        {
            this.condition = condition ?? throw new ArgumentNullException(nameof(condition));
            this.trueStep = trueStep ?? throw new ArgumentNullException(nameof(trueStep));
            this.falseStep = falseStep ?? throw new ArgumentNullException(nameof(falseStep));
        }

        /// <inheritdoc />
        protected override async Task ExecuteCoreAsync(IWorkflowContext context, CancellationToken cancellationToken)
        {
            step = condition(context) ? trueStep : falseStep;

            using (_ = logger.BeginScope(new Dictionary<string, object> {
                { "ParentStepId", StepId },
                { "ParentStepName", Name },
                { "StepId", step.StepId },
                { "StepName", step.Name }
            }))
            {
                await step.ExecuteAsync(context, cancellationToken).ConfigureAwait(true);
            }
        }

        /// <inheritdoc />
        protected override async Task CompensateCoreAsync(IWorkflowContext context, CancellationToken cancellationToken)
        {
            if (step != null)
            {
                using (_ = logger.BeginScope(new Dictionary<string, object> {
                    { "ParentStepId", StepId },
                    { "ParentStepName", Name },
                    { "StepId", step.StepId },
                    { "StepName", step.Name }
                }))
                {
                    await step.CompensateAsync(context, cancellationToken).ConfigureAwait(true);
                }
            }
        }

        /// <inheritdoc />
        protected override void DisposeCore()
        {
            trueStep.Dispose();
            falseStep.Dispose();
        }
    }
}