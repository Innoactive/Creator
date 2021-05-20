using VPG.Unity;

namespace VPG.Core
{
    /// <summary>
    /// Factory implementation for <see cref="IStep"/> objects.
    /// </summary>
    internal class StepFactory : Singleton<StepFactory>
    {
        /// <summary>
        /// Creates a new <see cref="IStep"/>.
        /// </summary>
        /// <param name="name"><see cref="IStep"/>'s name.</param>
        public IStep Create(string name)
        {
            return new Step(name);
        }
    }
}
