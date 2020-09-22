using System.Collections.Generic;
using System.Linq;
using Innoactive.Creator.Core;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Validation scope for steps.
    /// </summary>
    internal class StepValidationScope : BaseValidationScope<IStep, StepContext>
    {
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
