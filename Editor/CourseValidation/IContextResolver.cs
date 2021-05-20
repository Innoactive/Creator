using VPG.Core;

namespace VPG.Editor.CourseValidation
{
    /// <summary>
    /// Retrieves <see cref="IContext"/> from any provided <see cref="IData"/>.
    /// </summary>
    public interface IContextResolver
    {
        /// <summary>
        /// Resolves the fitting <see cref="IContext"/> for the given <see cref="IData"/>.
        /// </summary>
        IContext FindContext(IData data, ICourse course);
    }
}
