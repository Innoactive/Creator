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
        /// <param name="entity">Object, which will be validated.</param>
        /// <param name="course">Course where given <paramref name="entity"/> belongs.</param>
        /// <param name="context">Context of the validation.</param>
        /// <returns>List of reports regarding invalid objects related to the <paramref name="entity"/>.</returns>
        public ValidationReport Validate(IEntity entity, ICourse course, IContext context = null)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            if (context == null)
            {
                context = ContextResolver.FindContext(entity, course);
            }
            List<EditorReportEntry> entries = InternalValidate(entity, context);

            stopwatch.Stop();
            return new ValidationReport(entries, stopwatch.ElapsedMilliseconds);
        }

        /// <summary>
        /// Calls internal validation process for given <paramref name="entityObject"/>.
        /// </summary>
        /// <param name="entityObject">Object which is the target of the validation.</param>
        /// <param name="context">Context this validation runs in, has to be the correct one.</param>
        /// <returns>List of reports regarding invalid objects related to the <paramref name="entityObject"/>.</returns>
        protected List<EditorReportEntry> InternalValidate(object entityObject, IContext context)
        {
            List<EditorReportEntry> entries = new List<EditorReportEntry>();

            foreach (IValidationScope validation in activeValidations)
            {
                if (validation.CanValidate(entityObject))
                {
                    entries.AddRange(validation.Validate(entityObject, context));
                }
            }

            return entries;
        }
    }
}
