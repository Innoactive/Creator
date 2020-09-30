using System;
using System.Collections.Generic;
using System.Diagnostics;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Utils;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Helper class for triggering validations of <see cref="IValidationScope"/> objects.
    /// </summary>
    internal class ValidationHandler
    {
        private List<IValidationScope> activeValidations { get; } = new List<IValidationScope>();

        /// <summary>
        /// <see cref="IContextResolver"/> for resolving known context types.
        /// </summary>
        public IContextResolver ContextResolver { get; set; } = new ContextResolver();

        public ValidationHandler()
        {
            foreach (Type validatorType in ReflectionUtils.GetConcreteImplementationsOf<IValidationScope>())
            {
                activeValidations.Add((IValidationScope)ReflectionUtils.CreateInstanceOfType(validatorType));
            }
        }

        /// <summary>
        /// Validates the given object.
        /// </summary>
        /// <param name="obj">Object, which will be validated.</param>
        /// <param name="course">Course where given <paramref name="obj"/> belongs.</param>
        /// <param name="context">Context of the validation.</param>
        /// <returns>List of miss fits found while validating.</returns>
        public ValidationReport Validate(IEntity obj, ICourse course, IContext context = null)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            if (context == null)
            {
                context = ContextResolver.FindContext(obj, course);
            }
            List<ValidationReportEntry> entries = InternalValidate(obj, context);

            stopwatch.Stop();
            return new ValidationReport(entries, stopwatch.ElapsedMilliseconds);
        }

        /// <summary>
        /// Calls internal validation process for given <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">Object which is the target of the validation.</param>
        /// <param name="context">Context this validation runs in, has to be the correct one.</param>
        /// <returns>List of miss fits found while validating.</returns>
        protected List<ValidationReportEntry> InternalValidate(object obj, IContext context)
        {
            List<ValidationReportEntry> entries = new List<ValidationReportEntry>();

            foreach (IValidationScope validation in activeValidations)
            {
                if (validation.CanValidate(obj))
                {
                    entries.AddRange(validation.Validate(obj, context));
                }
            }

            return entries;
        }
    }
}
