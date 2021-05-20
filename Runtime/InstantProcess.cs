using System.Collections;

namespace VPG.Core
{
    /// <summary>
    /// A convenience class for processes that happen instantly. You only have to implement the <see cref="Start"/> method.
    /// </summary>
    public abstract class InstantProcess<TData> : Process<TData> where TData : class, IData
    {
        protected InstantProcess(TData data) : base(data)
        {
        }

        ///<inheritdoc />
        public abstract override void Start();

        ///<inheritdoc />
        public override IEnumerator Update()
        {
            yield break;
        }

        ///<inheritdoc />
        public override void End()
        {
        }

        ///<inheritdoc />
        public override void FastForward()
        {
        }
    }
}
