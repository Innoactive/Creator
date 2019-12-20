using System.Collections;
using Innoactive.Hub.Training.Configuration.Modes;

namespace Innoactive.Hub.Training.EntityOwners
{
    public abstract class EntityIteratingProcess<TData, TEntity> : IStageProcess<TData> where TData : IEntitySequenceData<TEntity>, IModeData where TEntity : IEntity
    {
        public virtual void Start(TData data)
        {
        }

        protected abstract bool ShouldActivateCurrent(TData data);

        protected abstract bool ShouldDeactivateCurrent(TData data);

        public IEnumerator Update(TData data)
        {
            TEntity current = default(TEntity);
            data.Current = current;

            while (TryNext(out current))
            {
                data.Current = current;

                if (data.Current == null)
                {
                    continue;
                }

                while (ShouldActivateCurrent(data) == false)
                {
                    yield return null;
                }

                data.Current.LifeCycle.Activate();

                if ((data.Current is IOptional && data.Mode.CheckIfSkipped(data.Current.GetType())))
                {
                    data.Current.LifeCycle.MarkToFastForward();
                }

                while (current.LifeCycle.Stage == Stage.Activating)
                {
                    yield return null;
                }

                while (ShouldDeactivateCurrent(data) == false)
                {
                    yield return null;
                }

                if (data.Current.LifeCycle.Stage != Stage.Inactive)
                {
                    data.Current.LifeCycle.Deactivate();
                }

                while (data.Current.LifeCycle.Stage != Stage.Inactive)
                {
                    yield return null;
                }
            }
        }

        public virtual void End(TData data)
        {
            data.Current = default(TEntity);
        }

        public virtual void FastForward(TData data)
        {
            TEntity current = data.Current;

            while (current != null || TryNext(out current))
            {
                data.Current = current;

                if (current.LifeCycle.Stage == Stage.Inactive)
                {
                    current.LifeCycle.Activate();
                }

                current.LifeCycle.MarkToFastForward();

                if (current.LifeCycle.Stage == Stage.Activating || current.LifeCycle.Stage == Stage.Active)
                {
                    current.LifeCycle.Deactivate();
                }

                current = default(TEntity);
            }
        }

        protected abstract bool TryNext(out TEntity entity);
    }
}
