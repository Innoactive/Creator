using System.Collections.Generic;

namespace VPG.Core.Validation
{
    /// <summary>
    /// Can be used to validate variables in any EntityData.
    /// </summary>
    public interface IAttributeValidator
    {
        /// <summary>
        /// ValidationState which will be used if the validation fails.
        /// </summary>
        ValidationErrorLevel ErrorLevel { get; }

        /// <summary>
        /// Runs the validation and returns true if there is a problem.
        /// </summary>
        /// <param name="value">Object which will be validated.</param>
        /// <returns>Returns a list of report entries.</returns>
        List<ReportEntry> Validate(object value);
    }
}
