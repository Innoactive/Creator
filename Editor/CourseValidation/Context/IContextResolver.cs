using Innoactive.Creator.Core;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Retrieves <see cref="IContext"/> from any provided <see cref="IEntity"/>.
    /// </summary>
    public interface IContextResolver
    {
        /// <summary>
        /// Resolves the fitting <see cref="IContext"/> for the given <see cref="IEntity"/>.
        /// </summary>
        IContext FindContext(IEntity entity, ICourse course);
    }
}
