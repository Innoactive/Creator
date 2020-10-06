using System.Collections.Generic;
using System.Linq;
using Innoactive.Creator.Core;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Validation scope for courses, will also validate all chapters of this course.
    /// </summary>
    internal class CourseValidationScope : BaseValidationScope<ICourseData, CourseContext>
    {
        protected ChapterValidationScope ChapterValidationScope { get; } = new ChapterValidationScope();

        /// <inheritdoc />
        protected override List<EditorReportEntry> InternalValidate(ICourseData course)
        {
            List<EditorReportEntry> report = new List<EditorReportEntry>();
            foreach (IValidator validator in Validators.Where(validator => validator.CanValidate(course)))
            {
                report.AddRange(validator.Validate(course, Context));
            }

            foreach (IChapter chapter in course.Chapters)
            {
                report.AddRange(ChapterValidationScope.Validate(chapter.Data, new ChapterContext(chapter.Data, Context)));
            }

            return report;
        }
    }
}
