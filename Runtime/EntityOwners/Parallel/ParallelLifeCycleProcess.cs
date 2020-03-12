using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Innoactive.Hub.Training.Configuration.Modes;

namespace Innoactive.Hub.Training.EntityOwners
{
    public class ParallelLifeCycleProcess<TData, TEntity> : Process<TData> where TEntity : IEntity where TData : IEntityCollectionData<TEntity>, IModeData
    {
        private static IEnumerable<TEntity> GetBlockingChildren(TData data)
        {
            return data.GetChildren()
                .Where(child => data.Mode.CheckIfSkipped(child.GetType()) == false)
                .Where(child =>
                {
                    IDataOwner dataOwner = child as IDataOwner;
                    if (dataOwner == null)
                    {
                        return true;
                    }

                    IBackgroundBehaviorData blockingData = dataOwner.Data as IBackgroundBehaviorData;
                    return blockingData == null || blockingData.IsBlocking;
                });
        }

        private class ActivatingProcess : IStageProcess<TData>
        {
            public void Start(TData data)
            {
                foreach (TEntity child in data.GetChildren().Where(child => data.Mode.CheckIfSkipped(child.GetType()) == false))
                {
                    child.LifeCycle.Activate();
                }
            }

            public IEnumerator Update(TData data)
            {
                while (GetBlockingChildren(data).Any(child => child.LifeCycle.Stage == Stage.Activating))
                {
                    yield return null;
                }
            }

            public void End(TData data)
            {
            }

            public void FastForward(TData data)
            {
                foreach (TEntity child in data.GetChildren().Where(child => child.LifeCycle.Stage == Stage.Activating))
                {
                    child.LifeCycle.MarkToFastForwardStage(Stage.Activating);
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
                int endlessIterationCheck = 0;
                while (endlessIterationCheck < 1000000)
                {
                    yield return null;
                    endlessIterationCheck++;
                }
            }

            public void End(TData data)
            {
            }

            public void FastForward(TData data)
            {
                foreach (TEntity child in data.GetChildren().Where(child => child.LifeCycle.Stage == Stage.Activating))
                {
                    child.LifeCycle.MarkToFastForwardStage(Stage.Activating);
                }
            }
        }

        private class DeactivatingProcess : IStageProcess<TData>
        {
            public void Start(TData data)
            {
                foreach (TEntity child in data.GetChildren().Where(child => data.Mode.CheckIfSkipped(child.GetType()) == false))
                {
                    child.LifeCycle.Deactivate();
                }
            }

            public IEnumerator Update(TData data)
            {
                while (GetBlockingChildren(data).Any(child => child.LifeCycle.Stage == Stage.Deactivating))
                {
                    yield return null;
                }
            }

            public void End(TData data)
            {
                foreach (TEntity child in data.GetChildren().Where(child => child.LifeCycle.Stage != Stage.Inactive))
                {
                    child.LifeCycle.MarkToFastForward();
                }
            }

            public void FastForward(TData data)
            {
            }
        }

        public ParallelLifeCycleProcess() : base(new ActivatingProcess(), new ActiveProcess(), new DeactivatingProcess())
        {
        }
    }
}
