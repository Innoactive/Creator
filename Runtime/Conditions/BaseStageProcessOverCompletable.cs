using System.Collections;

namespace Innoactive.Hub.Training.Conditions
{
    public abstract class BaseStageProcessOverCompletable<TData> : IStageProcess<TData> where TData : ICompletableData
    {
        public virtual void Start(TData data)
        {
            data.IsCompleted = false;
        }

        public virtual IEnumerator Update(TData data)
        {
            while (CheckIfCompleted(data) == false)
            {
                yield return null;
            }

            data.IsCompleted = true;
        }

        public virtual void End(TData data)
        {
        }

        public virtual void FastForward(TData data)
        {
            
        }

        protected abstract bool CheckIfCompleted(TData data);
    }
}
