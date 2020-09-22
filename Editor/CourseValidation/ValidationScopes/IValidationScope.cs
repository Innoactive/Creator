using System.Collections.Generic;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// ValidationScope limits the scope of the validation done. For example scopes are:
    /// * Course <see cref="CourseValidationScope"/>
    /// * Chapter <see cref="ChapterValidationScope"/>
    /// * Step <see cref="StepValidationScope"/>
    /// </summary>
    internal interface IValidationScope
    {
        /// <summary>
        /// Returns true if the object given can be validated by this scope.
        /// </summary>
        bool CanValidate(object obj);

        /// <summary>
        /// Validates the given object, the object has to be able to be validated.
        /// </summary>
        /// <param name="obj">Object which is the target of the validation.</param>
        /// <param name="context">Context this validation runs in, has to be the correct one.</param>
        /// <returns>List of miss fits found while validating.</returns>
        List<ValidationReportEntry> Validate(object obj, IContext context);
    }
}
