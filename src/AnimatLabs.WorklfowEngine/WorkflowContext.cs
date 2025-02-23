﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace AnimatLabs.WorkflowEngine
{
    /// <summary>
    /// Thread-safe context for sharing data between steps
    /// </summary>
    public class WorkflowContext : IWorkflowContext
    {
        private readonly ConcurrentDictionary<string, object> data;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowContext"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider for dependency injection.</param>
        /// <param name="workflowStepCompensationManager">The workflow step compensation manager.</param>
        /// <param name="workflow">The workflow instance.</param>
        /// <param name="correlationId">The correlation identifier for the workflow context.</param>
        public WorkflowContext(
            IServiceProvider serviceProvider,
            IWorkflowStepCompensationManager workflowStepCompensationManager,
            IWorkflow workflow,
            string correlationId = null)
        {
            CorrelationId = correlationId ?? Guid.NewGuid().ToString();
            data = new ConcurrentDictionary<string, object>();
            ServiceProvider = serviceProvider;
            WorkflowStepCompensationManager = workflowStepCompensationManager;
            Workflow = workflow;
        }

        /// <inheritdoc />
        public string CorrelationId { get; }


        /// <inheritdoc />
        public IServiceProvider ServiceProvider { get; }

        /// <inheritdoc />
        public IWorkflowStepCompensationManager WorkflowStepCompensationManager { get; }

        /// <inheritdoc />
        public IWorkflow Workflow { get; }

        /// <inheritdoc />
        public TValue GetValue<TValue>(string key)
        {
            if (data.TryGetValue(key, out var value))
            {
                return (TValue)value;
            }
            throw new KeyNotFoundException($"The key '{key}' was not found in the context.");
        }

        /// <inheritdoc />
        public TValue GetValueOrDefault<TValue>(string key) => data.TryGetValue(key, out var value) ? (TValue)value : default;

        /// <inheritdoc />
        public void SetValue<TValue>(string key, Func<TValue, TValue> value) =>
            data.AddOrUpdate(key, k => value(default), (k, v) => value((TValue)v));
    }
}