using System.Collections;

namespace Innoactive.Hub.Training.EntityOwners
{
    public class StopEntityIteratingProcess<TData, TEntity> : IStageProcess<TData> where TData : IEntitySequenceData<TEntity> where TEntity : IEntity
    {
        public void Start(TData data)
        {
        }

        public IEnumerator Update(TData data)
        {
            if (data.Current == null)
            {
                yield break;
            }

            if (data.Current.LifeCycle.Stage == Stage.Activating || data.Current.LifeCycle.Stage == Stage.Active)
            {
                data.Current.LifeCycle.Deactivate();
            }

            while (data.Current.LifeCycle.Stage != Stage.Inactive)
            {
                yield return null;
            }
        }

        public void End(TData data)
        {
            data.Current = default(TEntity);
        }

        public void FastForward(TData data)
        {
            if (data.Current != null)
            {
                data.Current.LifeCycle.MarkToFastForward();
            }
        }
    }
}
