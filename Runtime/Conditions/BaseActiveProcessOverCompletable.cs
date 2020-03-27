using System.Collections;

namespace Innoactive.Creator.Core.Conditions
{
    /// <summary>
    /// An abstract class for processes for Active <see cref="Stage"/> of <see cref="ICompletableEntity"/>.
    /// </summary>
    public abstract class BaseActiveProcessOverCompletable<TData> : Process<TData> where TData : class, ICompletableData
    {
        protected BaseActiveProcessOverCompletable(TData data) : base(data)
        {
        }

        /// <inheritdoc />
        public override void Start()
        {
            Data.IsCompleted = false;
        }

        /// <inheritdoc />
        public override IEnumerator Update()
        {
            while (CheckIfCompleted() == false)
            {
                yield return null;
            }

            Data.IsCompleted = true;
        }

        /// <inheritdoc />
        public override void End()
        {
        }

        /// <inheritdoc />
        public override void FastForward()
        {
        }

        /// <summary>
        /// Implement your custom check in this method. The process will not complete until this method returns true.
        /// </summary>
        protected abstract bool CheckIfCompleted();
    }
}
