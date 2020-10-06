using System.Linq;
using System.Collections.Generic;
using Innoactive.Creator.Core;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Validation scope for objects of type <see cref="IStep"/>.
    /// </summary>
    internal class StepValidationScope : BaseValidationScope<IStepData, StepContext>
    {
        /// <inheritdoc />
        protected override List<EditorReportEntry> InternalValidate(IStepData step)
        {
            List<EditorReportEntry> report = new List<EditorReportEntry>();

            foreach (IValidator validator in Validators.Where(validator => validator.CanValidate(step)))
            {
                report.AddRange(validator.Validate(step, Context));
            }

            return report;
        }
    }
}
