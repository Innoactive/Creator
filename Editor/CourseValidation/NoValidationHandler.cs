using System.Collections.Generic;
using Innoactive.Creator.Core;
using Innoactive.CreatorEditor.CourseValidation;

internal class NoValidationHandler : IValidationHandler
{
    public IContextResolver ContextResolver { get; set; } = null;

    public IValidationReport LastReport { get; } = null;

    public bool IsAllowedToValidate()
    {
        return false;
    }

    public IValidationReport Validate(IData data, ICourse course, IContext context = null)
    {
        return null;
    }
}
