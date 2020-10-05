namespace Innoactive.Creator.Core.Validation
{
    /// <summary>
    /// Base report entry with all information available non editor creator core.
    /// </summary>
    public class ReportEntry
    {
        /// <summary>
        /// Priority level for this <see cref="ValidationReportEntry"/>.
        /// </summary>
        public ValidationErrorLevel ErrorLevel;

        /// <summary>
        /// ErrorCode to easily identifying the error.
        /// </summary>
        public int Code;

        /// <summary>
        /// Detailed description of the issue.
        /// </summary>
        public string Message;
    }
}
