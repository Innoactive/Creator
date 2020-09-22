using Innoactive.Creator.Core.Validation;

namespace Innoactive.CreatorEditor.CourseValidation
{
    public class ValidationReportEntry
    {
        public ValidationErrorLevel ErrorLevel;
        public string Message;

        public IContext Context;
        public IValidator Validator;

        public override string ToString()
        {
            return $" [{ErrorLevel}]  [{Context}]: {Message}";
        }
    }
}
