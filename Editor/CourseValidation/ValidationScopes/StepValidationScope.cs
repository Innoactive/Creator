﻿using System.Linq;
using System.Collections.Generic;
using Innoactive.Creator.Core;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Validation scope for objects of type <see cref="IStep"/>.
    /// </summary>
    internal class StepValidationScope : BaseValidationScope<IStep, StepContext>
    {
        /// <inheritdoc />
        protected override List<ValidationReportEntry> InternalValidate(IStep step)
        {
            List<ValidationReportEntry> report = new List<ValidationReportEntry>();

            foreach (IValidator validator in Validators.Where(validator => validator.CanValidate(step)))
            {
                report.AddRange(validator.Validate(step, Context));
            }

            return report;
        }
    }
}