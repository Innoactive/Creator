using System;
using System.Collections.Generic;
using System.Diagnostics;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Utils;

namespace Innoactive.CreatorEditor.CourseValidation
{
    internal class ValidationHandler
    {
        private List<IValidationScope> activeValidations { get; } = new List<IValidationScope>();

        public IContextResolver ContextResolver { get; set; } = new ContextResolver();

        public ValidationHandler()
        {
            foreach (Type validatorType in ReflectionUtils.GetConcreteImplementationsOf<IValidationScope>())
            {
                activeValidations.Add((IValidationScope)ReflectionUtils.CreateInstanceOfType(validatorType));
            }
        }

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
