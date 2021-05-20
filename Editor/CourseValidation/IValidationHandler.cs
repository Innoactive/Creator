using System.Collections.Generic;
using VPG.Core;

namespace VPG.Editor.CourseValidation
{
    /// <summary>
    /// ValidationHandler validates data objects, e.g. steps or conditions, of a given course and reports whether
    /// the object causes issues or is faulty, e.g. empty fields or invalid values in a behavior.
    /// </summary>
    internal interface IValidationHandler
    {
        /// <summary>
        /// <see cref="IContextResolver"/> for resolving known context types.
        /// </summary>
        IContextResolver ContextResolver { get; set; }

        /// <summary>
        /// Last report generated.
        /// </summary>
        IValidationReport LastReport { get; }

        /// <summary>
        /// Checks if validation is currently allowed.
        /// </summary>
        bool IsAllowedToValidate();

        /// <summary>
        /// Validates the given object.
        /// </summary>
        /// <param name="data">Data object, which will be validated.</param>
        /// <param name="course">Course where given <paramref name="data"/> belongs.</param>
        /// <param name="context">Context of the validation.</param>
        /// <returns>List of reports regarding invalid objects related to the <paramref name="data"/>.</returns>
        IValidationReport Validate(IData data, ICourse course, IContext context = null);
    }
}
