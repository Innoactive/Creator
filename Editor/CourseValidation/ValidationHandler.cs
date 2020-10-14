using System;
using System.Collections.Generic;
using System.Diagnostics;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Configuration;
using Innoactive.Creator.Core.Utils;
using Innoactive.Creator.Unity;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Handles the continuous validation of given parts of a training course.
    /// </summary>
    internal class ValidationHandler : Singleton<ValidationHandler>
    {
        private List<IValidationScope> activeValidations { get; } = new List<IValidationScope>();

        /// <summary>
        /// <see cref="IContextResolver"/> for resolving known context types.
        /// </summary>
        public IContextResolver ContextResolver { get; set; } = new ContextResolver();

        /// <summary>
        /// Last report generated.
        /// </summary>
        public ValidationReport LastReport { get; protected set; }

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
        /// <param name="data">Data object, which will be validated.</param>
        /// <param name="course">Course where given <paramref name="data"/> belongs.</param>
        /// <param name="context">Context of the validation.</param>
        /// <returns>List of reports regarding invalid objects related to the <paramref name="data"/>.</returns>
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
        /// Checks if validation is currently allowed.
        /// </summary>
        public static bool IsAllowedToValidate()
        {
            return CreatorProjectSettings.Load().IsValidationEnabled
                   && RuntimeConfigurator.Exists
                   && Application.isPlaying == false
                   && EditorApplication.isCompiling == false;
        }

        /// <summary>
        /// Calls internal validation process for given <paramref name="validatableObject"/>.
        /// </summary>
        /// <param name="validatableObject">Object which is the target of the validation.</param>
        /// <param name="context">Context this validation runs in, has to be the correct one.</param>
        /// <returns>List of reports regarding invalid objects related to the <paramref name="validatableObject"/>.</returns>
        protected List<EditorReportEntry> InternalValidate(object validatableObject, IContext context)
        {
            List<EditorReportEntry> entries = new List<EditorReportEntry>();

            foreach (IValidationScope validation in activeValidations)
            {
                if (validation.CanValidate(validatableObject))
                {
                    entries.AddRange(validation.Validate(validatableObject, context));
                }
            }

            return entries;
        }
    }
}
