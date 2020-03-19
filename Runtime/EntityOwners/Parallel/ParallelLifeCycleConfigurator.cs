using Innoactive.Creator.Core.Configuration.Modes;

namespace Innoactive.Creator.Core.EntityOwners
{
    public class ParallelLifeCycleConfigurator<TData, TEntity> : IConfigurator<TData> where TEntity : IEntity where TData : IEntityCollectionData<TEntity>, IModeData
    {
        public void Configure(TData data, IMode mode, Stage stage)
        {
            foreach (TEntity child in data.GetChildren())
            {
                if (child is IOptional)
                {
                    bool wasSkipped = data.Mode != null && data.Mode.CheckIfSkipped(child.GetType());
                    bool isSkipped = mode.CheckIfSkipped(child.GetType());

                    if (wasSkipped == isSkipped)
                    {
                        continue;
                    }

                    if (isSkipped)
                    {
                        if (child.LifeCycle.Stage == Stage.Inactive)
                        {
                            continue;
                        }

                        child.LifeCycle.MarkToFastForward();

                        if (child.LifeCycle.Stage == Stage.Active)
                        {
                            child.LifeCycle.Deactivate();
                        }
                    }
                    else
                    {
                        if (stage == Stage.Deactivating)
                        {
                            child.LifeCycle.MarkToFastForwardStage(Stage.Activating);
                            child.LifeCycle.MarkToFastForwardStage(Stage.Active);
                        }

                        if (stage == Stage.Activating || stage == Stage.Active)
                        {
                            child.LifeCycle.Activate();
                        }
                    }
                }

                child.Configure(mode);
            }
        }
    }
}
