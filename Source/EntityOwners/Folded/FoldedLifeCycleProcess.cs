using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Innoactive.Hub.Training.Configuration.Modes;

namespace Innoactive.Hub.Training.EntityOwners
{
    public class FoldedLifeCycleProcess<TData, TEntity> : Process<TData> where TData : IEntitySequenceData<TEntity>, IModeData where TEntity : IEntity
    {
        private class ActivatingProcess : IStageProcess<TData>
        {
            private IEnumerator<TEntity> enumerator;

            public void Start(TData data)
            {
                enumerator = data.GetChildren().GetEnumerator();
                enumerator.Reset();
            }

            public IEnumerator Update(TData data)
            {
                while (enumerator.MoveNext())
                {
                    data.Current = enumerator.Current;

                    if (data.Current == null)
                    {
                        continue;
                    }

                    data.Current.LifeCycle.Activate();

                    if (data.Current.LifeCycle.Stage == Stage.Activating && data.Mode.CheckIfSkipped(data.Current.GetType()))
                    {
                        data.Current.LifeCycle.MarkToFastForwardStage(Stage.Activating);
                    }

                    while (data.Current.LifeCycle.Stage != Stage.Active)
                    {
                        yield return null;
                    }
                }
            }

            public void End(TData data)
            {
                enumerator = null;
            }

            public void FastForward(TData data)
            {
                if (Equals(data.Current, default(TEntity)))
                {
                    if (enumerator.MoveNext())
                    {
                        data.Current = enumerator.Current;
                    }
                }

                while (Equals(data.Current, default(TEntity)) == false)
                {
                    if (data.Current.LifeCycle.Stage == Stage.Inactive)
                    {
                        data.Current.LifeCycle.Activate();
                    }

                    if (data.Current.LifeCycle.Stage == Stage.Activating)
                    {
                        data.Current.LifeCycle.MarkToFastForwardStage(Stage.Activating);
                    }

                    data.Current = enumerator.MoveNext() ? enumerator.Current : default(TEntity);
                }
            }
        }

        private class ActiveProcess : IStageProcess<TData>
        {
            public void Start(TData data)
            {
            }

            public IEnumerator Update(TData data)
            {
                foreach (TEntity child in data.GetChildren()
                    .Where(child => child.LifeCycle.Stage == Stage.Active)
                    .Where(child => data.Mode.CheckIfSkipped(child.GetType())))
                {
                    child.LifeCycle.MarkToFastForwardStage(Stage.Active);
                }

                yield break;
            }

            public void End(TData data)
            {
            }

            public void FastForward(TData data)
            {
                foreach (TEntity child in data.GetChildren())
                {
                    if (child.LifeCycle.Stage == Stage.Active)
                    {
                        child.LifeCycle.MarkToFastForwardStage(Stage.Active);
                    }
                }
            }
        }

        private class DeactivatingProcess : IStageProcess<TData>
        {
            private IEnumerator<TEntity> enumerator;

            public void Start(TData data)
            {
                enumerator = data.GetChildren().Reverse().GetEnumerator();
            }

            public IEnumerator Update(TData data)
            {
                while (enumerator.MoveNext())
                {
                    data.Current = enumerator.Current;

                    if (data.Current == null)
                    {
                        continue;
                    }

                    if (data.Current.LifeCycle.Stage != Stage.Inactive)
                    {
                        data.Current.LifeCycle.Deactivate();
                    }

                    if (data.Current.LifeCycle.Stage == Stage.Deactivating && data.Mode.CheckIfSkipped(data.Current.GetType()))
                    {
                        data.Current.LifeCycle.MarkToFastForwardStage(Stage.Deactivating);
                    }

                    while (data.Current.LifeCycle.Stage != Stage.Inactive)
                    {
                        yield return null;
                    }
                }
            }

            public void End(TData data)
            {
                enumerator = null;
            }

            public void FastForward(TData data)
            {
                if (Equals(data.Current, default(TEntity)))
                {
                    if (enumerator.MoveNext())
                    {
                        data.Current = enumerator.Current;
                    }
                }

                while (Equals(data.Current, default(TEntity)) == false)
                {
                    if (data.Current.LifeCycle.Stage == Stage.Active)
                    {
                        data.Current.LifeCycle.Deactivate();
                    }

                    if (data.Current.LifeCycle.Stage == Stage.Deactivating)
                    {
                        data.Current.LifeCycle.MarkToFastForwardStage(Stage.Deactivating);
                    }

                    data.Current = enumerator.MoveNext() ? enumerator.Current : default(TEntity);
                }
            }
        }

        public FoldedLifeCycleProcess() : base(new ActivatingProcess(), new ActiveProcess(), new DeactivatingProcess())
        {
        }
    }
}
