using Innoactive.Creator.Core.Validation;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Report generated from validations of <see cref="IValidator"/> objects.
    /// </summary>
    public class EditorReportEntry : ReportEntry
    {
        /// <summary>
        /// <see cref="IContext"/> where the issue is present.
        /// </summary>
        public IContext Context;

        /// <summary>
        /// <see cref="IValidator"/> used to generate this <see cref="EditorReportEntry"/>.
        /// </summary>
        public IValidator Validator;

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return $" [{ErrorLevel}]  [{Context}]: {Message}";
        }
    }
}
