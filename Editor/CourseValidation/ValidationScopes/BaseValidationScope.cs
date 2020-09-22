using System;
using System.Collections.Generic;
using System.Linq;
using Innoactive.Creator.Core.Utils;
using UnityEngine;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Base class for extension, dont use <see cref="IValidationScope"/> which is used for internal purpose.
    /// </summary>
    /// <typeparam name="T">The type of object which will be validated by this validation scope</typeparam>
    /// <typeparam name="TContext">Context this validation scope is working in.</typeparam>
    public abstract class BaseValidationScope<T, TContext> : IValidationScope where TContext : IContext
    {
    protected TContext Context { get; private set; }

    protected List<IValidator> Validators { get; }

    protected BaseValidationScope()
    {
        Validators = GetValidators();
    }

    /// <summary>
    /// Creates the initial list of validators for this scope, can be overwritten to change this otherwise this will be
    /// done by reflection.
    /// </summary>
    protected virtual List<IValidator> GetValidators()
    {
        return ReflectionUtils.GetConcreteImplementationsOf<IValidator>()
            .Select(type => (IValidator) ReflectionUtils.CreateInstanceOfType(type))
            .Where(validator => validator.ValidatedType == typeof(T))
            .ToList();
    }

    /// <inheritdoc />
    public bool CanValidate(object obj)
    {
        return typeof(T).IsAssignableFrom(obj.GetType());
    }

    /// <summary>
    /// Prepares the validation, sets the context and calls InternalValidate.
    /// </summary>
    /// <param name="obj">Object which will be validated, has to be Type T.</param>
    /// <param name="context">Context we are working in, has to be TContext.</param>
    /// <returns>All found miss fits for the given objects.</returns>
    /// <exception cref="InvalidCastException">Will be thrown when the object is not of Type T</exception>
    public List<ValidationReportEntry> Validate(object obj, IContext context)
    {
        if (!(context is TContext))
        {
            Debug.LogWarning($"Context given to this validation scope is wrong it is {context.GetType()} but should be {typeof(TContext).Name}");
            Context = default(TContext);
        }
        else
        {
            Context = (TContext) context;
        }

        if (CanValidate(obj))
        {
            return InternalValidate((T) obj);
        }

        throw new InvalidCastException();
    }

    /// <summary>
    /// Run your validators here.
    /// </summary>
    /// <param name="obj">Object which will be validated, has to be Type T.</param>
    /// <returns>All found miss fits for the given objects.</returns>
    protected abstract List<ValidationReportEntry> InternalValidate(T obj);
    }
}
