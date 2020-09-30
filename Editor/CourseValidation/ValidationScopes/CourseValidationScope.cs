using System.Collections.Generic;
using System.Linq;
using Innoactive.Creator.Core;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Validation scope for courses, will also validate all chapters of this course.
    /// </summary>
    internal class CourseValidationScope : BaseValidationScope<ICourse, CourseContext>
    {
        protected ChapterValidationScope ChapterValidationScope { get; } = new ChapterValidationScope();

        /// <inheritdoc />
        protected override List<ValidationReportEntry> InternalValidate(ICourse course)
        {
            List<ValidationReportEntry> report = new List<ValidationReportEntry>();
            foreach (IValidator validator in Validators.Where(validator => validator.CanValidate(course)))
            {
                report.AddRange(validator.Validate(course, Context));
            }

            foreach (IChapter chapter in course.Data.Chapters)
            {
                report.AddRange(ChapterValidationScope.Validate(chapter, new ChapterContext(chapter, Context)));
            }

            return report;
        }
    }
}
