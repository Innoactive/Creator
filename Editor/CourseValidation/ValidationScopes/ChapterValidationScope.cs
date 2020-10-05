using System.Linq;
using System.Collections.Generic;
using Innoactive.Creator.Core;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Validation scope for chapters, will also validate all steps contained in this chapter.
    /// </summary>
    internal class ChapterValidationScope : BaseValidationScope<IChapter, ChapterContext>
    {
        protected StepValidationScope StepValidationScope { get; } = new StepValidationScope();

        /// <inheritdoc />
        protected override List<EditorReportEntry> InternalValidate(IChapter chapter)
        {
            List<EditorReportEntry> report = new List<EditorReportEntry>();
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
