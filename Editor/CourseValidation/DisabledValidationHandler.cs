using VPG.Core;
using VPG.Editor.CourseValidation;

/// <summary>
/// Does not validate, used to disabled the validation system.
/// </summary>
internal class DisabledValidationHandler : IValidationHandler
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
