using System;
using System.Linq;
using System.Collections.Generic;
using Innoactive.Creator.Core.Utils;
using UnityEngine;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Base class for extension, do not use <see cref="IValidationScope"/> which is used for internal purpose.
    /// </summary>
    /// <typeparam name="T">The type of object which will be validated by this validation scope</typeparam>
    /// <typeparam name="TContext">Context this validation scope is working in.</typeparam>
    public abstract class BaseValidationScope<T, TContext> : IValidationScope where TContext : IContext
    {
        /// <summary>
        /// <see cref="TContext"/> used for this <see cref="IValidationScope"/>.
        /// </summary>
        protected TContext Context { get; private set; }

        /// <summary>
        /// List of <see cref="IValidator"/> used in this <see cref="IValidationScope"/>.
        /// </summary>
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
        public bool CanValidate(object entityObject)
        {
            return entityObject is T;
        }

        /// <summary>
        /// Prepares the validation, sets the context and calls InternalValidate.
        /// </summary>
        /// <param name="entityObject">Object which will be validated, has to be Type T.</param>
        /// <param name="context">Context we are working in, has to be TContext.</param>
        /// <returns>List of reports regarding invalid objects related to the <paramref name="entityObject"/>.</returns>
        /// <exception cref="InvalidCastException">Will be thrown when the object is not of Type T</exception>
        public List<ValidationReportEntry> Validate(object entityObject, IContext context)
        {
            if (context is TContext == false)
            {
                Debug.LogWarning($"Context given to this validation scope is wrong it is {context.GetType()} but should be {typeof(TContext).Name}");
                Context = default;
            }
            else
            {
                Context = (TContext) context;
            }

            if (CanValidate(entityObject))
            {
                return InternalValidate((T) entityObject);
            }

            throw new InvalidCastException();
        }

        /// <summary>
        /// Validates given object.
        /// </summary>
        /// <remarks>Run your validators here.</remarks>
        /// <param name="entityObject">Object which will be validated, has to be Type T.</param>
        /// <returns>List of reports regarding invalid objects related to the <paramref name="entityObject"/>.</returns>
        protected abstract List<ValidationReportEntry> InternalValidate(T entityObject);
    }
}
