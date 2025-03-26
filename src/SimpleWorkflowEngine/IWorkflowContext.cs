using System;
using System.Collections.Generic;
using SimpleWorkflowEngine;

/// <summary>
/// Interface for WorkflowContext
/// </summary>
public interface IWorkflowContext
{
    /// <summary>
    /// Gets the correlation identifier for the workflow context.
    /// </summary>
    string CorrelationId { get; }

    /// <summary>
    /// Gets the service provider.
    /// </summary>
    IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Gets the workflow step compensation manager.
    /// </summary>
    IWorkflowStepCompensationManager WorkflowStepCompensationManager { get; }

    /// <summary>
    /// Gets the workflow instance.
    /// </summary>
    IWorkflow Workflow { get; }

    /// <summary>
    /// Retrieves a value from the context or throws an exception if not found.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="key">The key of the value.</param>
    /// <returns>The value associated with the key.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the <paramref name="key"/> is not found in the context.</exception>
    TValue GetValue<TValue>(string key);

    /// <summary>
    /// Retrieves a value from the context or returns default if not found.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="key">The key of the value.</param>
    /// <returns>The value associated with the key, or default if not found.</returns>
    TValue GetValueOrDefault<TValue>(string key);

    /// <summary>
    /// Sets a value in the context.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="key">The key of the value.</param>
    /// <param name="value">The value to set.</param>
    void SetValue<TValue>(string key, Func<TValue, TValue> value);
}