using VPG.Creator.Unity;

namespace VPG.Creator.Core
{
    /// <summary>
    /// Factory implementation for <see cref="ICourse"/> objects.
    /// </summary>
    internal class CourseFactory : Singleton<CourseFactory>
    {
        /// <summary>
        /// Creates a new <see cref="ICourse"/>.
        /// </summary>
        /// <param name="name"><see cref="ICourse"/>'s name.</param>
        /// <param name="firstStep">Initial <see cref="IStep"/> for this <see cref="ICourse"/>.</param>
        public ICourse Create(string name, IStep firstStep = null)
        {
            return new Course(name, new Chapter("Chapter 1", firstStep));
        }
    }
}
