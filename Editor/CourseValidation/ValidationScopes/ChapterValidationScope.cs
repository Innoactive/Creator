using System.Collections.Generic;
using System.Linq;
using Innoactive.Creator.Core;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Validation scope for chapters, will also validate all steps contained in this chapter.
    /// </summary>
    internal class ChapterValidationScope : BaseValidationScope<IChapter, ChapterContext>
    {
        protected StepValidationScope StepValidationScope { get; } = new StepValidationScope();

        protected override List<ValidationReportEntry> InternalValidate(IChapter chapter)
        {
            List<ValidationReportEntry> report = new List<ValidationReportEntry>();
            foreach (IValidator validator in Validators.Where(validator => validator.CanValidate(chapter)))
            {
                report.AddRange(validator.Validate(chapter, Context));
            }

            foreach (IStep step in chapter.Data.Steps)
            {
                report.AddRange(StepValidationScope.Validate(step, new StepContext(step, Context)));
            }

            return report;
        }
    }
}
