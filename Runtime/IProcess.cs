using System.Collections;

namespace Innoactive.Creator.Core
{
    /// <summary>
    /// A process for an <see cref="IEntity"/>'s <see cref="Stage"/>.
    /// </summary>
    public interface IProcess
    {
        /// <summary>
        /// This method is invoked immediately when entity enters the stage.
        /// The invocation is guaranteed.
        /// Use it for initialization.
        /// </summary>
        void Start();

        /// <summary>
        /// This method will be iterated over while the entity is in this stage, one iteration step per frame, starting from the second frame.
        /// </summary>
        /// <returns></returns>
        IEnumerator Update();

        /// <summary>
        /// This method is invoked immediately after the <see cref="Update"/> was iterated over completely, or after the <see cref="FastForward"/> was called.
        /// The invocation is guaranteed.
        /// Use it for deinitialization.
        /// </summary>
        void End();

        /// <summary>
        /// This method is called when the process was not completed yet.
        /// It must "fake" normal execution of the process and handle the cases when the <see cref="Update"/> is not iterated over completely.
        /// </summary>
        void FastForward();
    }
}
