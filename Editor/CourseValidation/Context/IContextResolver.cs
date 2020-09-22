using Innoactive.Creator.Core;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Allows to resolve a <see cref="IContext"/> out of a given <see cref="IEntity"/>.
    /// </summary>
    public interface IContextResolver
    {
        /// <summary>
        /// Resolves the fitting <see cref="IContext"/> for the given <see cref="IEntity"/>.
        /// </summary>
        IContext FindContext(IEntity entity, ICourse course);
    }
}
