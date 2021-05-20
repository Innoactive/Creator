using VPG.Core.Validation;

namespace VPG.Editor.CourseValidation
{
    /// <summary>
    /// Report generated from validations of <see cref="IValidator"/> objects.
    /// </summary>
    public class EditorReportEntry : ReportEntry
    {
        /// <summary>
        /// <see cref="IContext"/> where the issue is present.
        /// </summary>
        public readonly IContext Context;

        /// <summary>
        /// <see cref="IValidator"/> used to generate this <see cref="EditorReportEntry"/>.
        /// </summary>
        public readonly IValidator Validator;

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return $" [{ErrorLevel}]  [{Context}]: {Message}";
        }

        public EditorReportEntry(IContext context, IValidator validator, ReportEntry entry) : base(entry)
        {
            Context = context;
            Validator = validator;
        }
    }
}
