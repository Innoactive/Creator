using System;
using System.Collections.Generic;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Validator provide validation for an specific Type.
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
        bool CanValidate(object obj);

        /// <summary>
        /// Validates the given object.
        /// </summary>
        /// <param name="obj">Object, which will be validated.</param>
        /// <param name="context">Context of the validation.</param>
        /// <returns>List of miss fits found while validating.</returns>
        List<ValidationReportEntry> Validate(object obj, IContext context);
    }
}
