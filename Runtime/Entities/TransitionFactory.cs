using VPG.Creator.Unity;

namespace VPG.Creator.Core
{
    /// <summary>
    /// Factory implementation for <see cref="ITransition"/> objects.
    /// </summary>
    internal class TransitionFactory : Singleton<TransitionFactory>
    {
        /// <summary>
        /// Creates a new <see cref="ITransition"/>.
        /// </summary>
        public ITransition Create()
        {
            return new Transition();
        }
    }
}
