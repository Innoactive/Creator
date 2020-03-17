using System.Collections;

namespace Innoactive.Creator.Core
{
    public abstract class InstantStageProcess<TData> : IStageProcess<TData> where TData : IData
    {
        public abstract void Start(TData data);

        public IEnumerator Update(TData data)
        {
            yield break;
        }

        public void End(TData data)
        {
        }

        public void FastForward(TData data)
        {
        }
    }
}
