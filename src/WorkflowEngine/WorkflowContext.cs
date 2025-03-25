using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using WorkflowEngine.Core;

namespace WorkflowEngine
{
    /// <summary>
    /// Represents a thread-safe context for sharing data between workflow steps.
    /// </summary>
    public class WorkflowContext : IWorkflowContext
    {
        private readonly ConcurrentDictionary<string, object> data;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowContext"/> class.
        /// </summary>
        /// <param name="workflow">The workflow instance.</param>
        /// <param name="correlationId">The correlation identifier for the workflow context.</param>
        /// <param name="serviceProvider">The service provider for dependency injection.</param>
        /// <param name="workflowStepCompensationManager">The workflow step compensation manager.</param>
        public WorkflowContext(
            IWorkflow workflow,
            string correlationId = null,
            IServiceProvider serviceProvider = null,
            IWorkflowStepCompensationManager workflowStepCompensationManager = null)
        {
            Workflow = workflow;
            data = new ConcurrentDictionary<string, object>();
            CorrelationId = correlationId ?? Guid.NewGuid().ToString();
            ServiceProvider = serviceProvider;
            WorkflowStepCompensationManager = workflowStepCompensationManager ?? new WorkflowStepCompensationManager();
        }

        /// <inheritdoc/>
        public string CorrelationId { get; }

        /// <inheritdoc/>
        public IServiceProvider ServiceProvider { get; }

        /// <inheritdoc/>
        public IWorkflowStepCompensationManager WorkflowStepCompensationManager { get; }

        /// <inheritdoc/>
        public IWorkflow Workflow { get; }

        /// <inheritdoc/>
        public TValue GetValue<TValue>(string key)
        {
            if (data.TryGetValue(key, out var value))
            {
                return (TValue)value;
            }
            throw new KeyNotFoundException($"The key '{key}' was not found in the context.");
        }

        /// <inheritdoc/>
        public TValue GetValueOrDefault<TValue>(string key) => data.TryGetValue(key, out var value) ? (TValue)value : default;

        /// <inheritdoc/>
        public void SetValue<TValue>(string key, Func<TValue, TValue> value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));
            data.AddOrUpdate(key, k => value(default), (k, v) => value((TValue)v));
        }
    }
}