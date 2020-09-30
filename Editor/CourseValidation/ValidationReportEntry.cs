using Innoactive.Creator.Core.Validation;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Report generated from validations of <see cref="IValidator"/> objects.
    /// </summary>
    public class ValidationReportEntry
    {
        /// <summary>
        /// Priority level for this <see cref="ValidationReportEntry"/>.
        /// </summary>
        public ValidationErrorLevel ErrorLevel;

        /// <summary>
        /// Detailed description of the issue.
        /// </summary>
        public string Message;

        /// <summary>
        /// <see cref="IContext"/> where the issue is present.
        /// </summary>
        public IContext Context;

        /// <summary>
        /// <see cref="IValidator"/> used to generate this <see cref="ValidationReportEntry"/>.
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
