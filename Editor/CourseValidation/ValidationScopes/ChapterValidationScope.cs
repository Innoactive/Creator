using System.Linq;
using System.Collections.Generic;
using Innoactive.Creator.Core;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Validation scope for chapters, will also validate all steps contained in this chapter.
    /// </summary>
    internal class ChapterValidationScope : BaseValidationScope<IChapterData, ChapterContext>
    {
        protected StepValidationScope StepValidationScope { get; } = new StepValidationScope();

        /// <inheritdoc />
        protected override List<EditorReportEntry> InternalValidate(IChapterData chapter)
        {
            List<EditorReportEntry> report = new List<EditorReportEntry>();
            foreach (IValidator validator in Validators.Where(validator => validator.CanValidate(chapter)))
            {
                report.AddRange(validator.Validate(chapter, Context));
            }

            foreach (IStep step in chapter.Steps)
            {
                report.AddRange(StepValidationScope.Validate(step.Data, new StepContext(step.Data, Context)));
            }

            return report;
        }
    }
}
