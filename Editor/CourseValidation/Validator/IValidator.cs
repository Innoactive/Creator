using System;
using System.Collections.Generic;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Validator provides validation for a specific Type.
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// Type which is validated by this validator.
        /// </summary>
        Type ValidatedType { get; }

        /// <summary>
        /// Will return true when the object can be validated by this validator.
        /// </summary>
        /// <param name="entityObject">Object to validate.</param>
        /// <returns>True if object can be validated.</returns>
        bool CanValidate(object entityObject);

        /// <summary>
        /// Validates the given object.
        /// </summary>
        /// <param name="entityObject">Object, which will be validated.</param>
        /// <param name="context">Context of the validation.</param>
        /// <returns>List of reports regarding invalid objects related to the <paramref name="entityObject"/>.</returns>
        List<ValidationReportEntry> Validate(object entityObject, IContext context);
    }
}
