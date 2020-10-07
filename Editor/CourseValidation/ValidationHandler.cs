﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Configuration;
using Innoactive.Creator.Core.Utils;
using Innoactive.Creator.Unity;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Helper class for triggering validations of <see cref="IValidationScope"/> objects.
    /// </summary>
    internal class ValidationHandler : Singleton<ValidationHandler>
    {
        private List<IValidationScope> activeValidations { get; } = new List<IValidationScope>();

        /// <summary>
        /// <see cref="IContextResolver"/> for resolving known context types.
        /// </summary>
        public IContextResolver ContextResolver { get; set; } = new ContextResolver();

        public ValidationReport LastReport { get; protected set; }

        public ValidationHandler()
        {
            foreach (Type validatorType in ReflectionUtils.GetConcreteImplementationsOf<IValidationScope>())
            {
                activeValidations.Add((IValidationScope)ReflectionUtils.CreateInstanceOfType(validatorType));
            }
        }

        public void ClearReport()
        {
            LastReport = null;
        }

        /// <summary>
        /// Validates the given object.
        /// </summary>
        /// <param name="entity">Object, which will be validated.</param>
        /// <param name="course">Course where given <paramref name="entity"/> belongs.</param>
        /// <param name="context">Context of the validation.</param>
        /// <returns>List of reports regarding invalid objects related to the <paramref name="entity"/>.</returns>
        public ValidationReport Validate(IData data, ICourse course, IContext context = null)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            if (context == null)
            {
                context = ContextResolver.FindContext(data, course);
            }
            List<EditorReportEntry> entries = InternalValidate(data, context);

            stopwatch.Stop();
            if (LastReport == null)
            {
                LastReport = new ValidationReport(entries, stopwatch.ElapsedMilliseconds);
            }
            else
            {
                LastReport.Update(entries, context, stopwatch.ElapsedMilliseconds);
            }
            return LastReport;
        }

        /// <summary>
        /// Calls internal validation process for given <paramref name="validateableObject"/>.
        /// </summary>
        /// <param name="validateableObject">Object which is the target of the validation.</param>
        /// <param name="context">Context this validation runs in, has to be the correct one.</param>
        /// <returns>List of reports regarding invalid objects related to the <paramref name="validateableObject"/>.</returns>
        protected List<EditorReportEntry> InternalValidate(object validateableObject, IContext context)
        {
            List<EditorReportEntry> entries = new List<EditorReportEntry>();

            foreach (IValidationScope validation in activeValidations)
            {
                if (validation.CanValidate(validateableObject))
                {
                    entries.AddRange(validation.Validate(validateableObject, context));
                }
            }

            return entries;
        }
    }
}
