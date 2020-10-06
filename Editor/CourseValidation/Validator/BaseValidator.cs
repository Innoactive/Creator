using System;
using System.Collections.Generic;
using UnityEngine;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Base validator class to create a new validator.
    /// </summary>
    /// <typeparam name="T">Type of the objects which will be validated by this validator.</typeparam>
    /// <typeparam name="TContext">Context type the objects will be validated in.</typeparam>
    public abstract class BaseValidator<T, TContext> : IValidator where TContext : IContext
    {
        /// <inheritdoc/>
        public Type ValidatedType => typeof(T);

        /// <summary>
        /// Current Context we are in.
        /// </summary>
        protected TContext Context { get; private set; }

        /// <inheritdoc/>
        public bool CanValidate(object validatableObject)
        {
            return ValidatedType.IsInstanceOfType(validatableObject);
        }

        /// <summary>
        /// Prepares the validation, sets the context and calls InternalValidate.
        /// </summary>
        /// <param name="validatableObject">Object which will be validated, has to be Type T.</param>
        /// <param name="context">Context we are working in, has to be TContext.</param>
        /// <returns>List of reports regarding invalid objects related to the <paramref name="validatableObject"/>.</returns>
        /// <exception cref="InvalidCastException">Will be thrown when the object is not of Type T</exception>
        public List<EditorReportEntry> Validate(object validatableObject, IContext context)
        {
            try
            {
                if ((context is TContext) == false)
                {
                    Debug.LogWarning($"Context given to this validation scope is wrong it is {context.GetType()} but should be {typeof(TContext).Name}");
                    Context = default(TContext);
                }
                else
                {
                    Context = (TContext) context;
                }

                if (ValidatedType.IsInstanceOfType(validatableObject))
                {
                    return InternalValidate((T) validatableObject);
                }

                throw new InvalidOperationException();
            }
            catch (Exception ex)
            {
                Debug.LogError($"{ex.GetType().Name} while trying to validate: \n{ex.StackTrace}");
                return new List<EditorReportEntry>();
            }
        }

        /// <summary>
        /// Implement your validation here.
        /// </summary>
        /// <param name="step">Object which will be validated, has to be Type T.</param>
        /// <returns>List of reports regarding invalid objects related to the <paramref name="step"/>.</returns>
        protected abstract List<EditorReportEntry> InternalValidate(T step);
    }
}
