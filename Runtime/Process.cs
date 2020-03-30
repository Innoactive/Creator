using System.Collections;

namespace Innoactive.Creator.Core
{
    /// <summary>
    /// A base implementation of a <seealso cref="IProcess"/> which provides access to its entity's data.
    /// </summary>
    public abstract class Process<TData> : IProcess where TData : class, IData
    {
        /// <summary>
        /// The entity's data.
        /// </summary>
        protected TData Data { get; }

        protected Process(TData data)
        {
            Data = data;
        }

        /// <inheritdoc />
        public abstract void Start();

        /// <inheritdoc />
        public abstract IEnumerator Update();

        /// <inheritdoc />
        public abstract void End();

        /// <inheritdoc />
        public abstract void FastForward();
    }
}
