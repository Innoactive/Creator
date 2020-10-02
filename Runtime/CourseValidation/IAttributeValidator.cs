namespace Innoactive.Creator.Core.Validation
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
        /// <param name="message">Message which will be used if there is a problem.</param>
        /// <returns>Return true if the validation fails for the given object.</returns>
        bool Validate(object value, out string message);
    }
}
