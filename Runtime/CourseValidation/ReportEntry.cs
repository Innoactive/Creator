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
        public readonly ValidationErrorLevel ErrorLevel;

        /// <summary>
        /// ErrorCode to easily identifying the error.
        /// </summary>
        public readonly int Code;

        /// <summary>
        /// Detailed description of the issue.
        /// </summary>
        public readonly string Message;

        public ReportEntry(int code, string message, ValidationErrorLevel errorLevel)
        {
            Code = code;
            Message = message;
            ErrorLevel = errorLevel;
        }

        protected ReportEntry(ReportEntry entry)
        {
            Code = entry.Code;
            Message = entry.Message;
            ErrorLevel = entry.ErrorLevel;
        }
    }
}
