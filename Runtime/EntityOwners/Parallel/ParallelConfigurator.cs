using VPG.Creator.Core.Configuration.Modes;

namespace VPG.Creator.Core.EntityOwners
{
    /// <summary>
    /// A configurator for a collection of entities.
    /// </summary>
    public class ParallelConfigurator<TEntity> : Configurator<IEntityCollectionDataWithMode<TEntity>> where TEntity : IEntity
    {
        public ParallelConfigurator(IEntityCollectionDataWithMode<TEntity> data) : base(data)
        {
        }

        /// <inheritdoc />
        public override void Configure(IMode mode, Stage stage)
        {
            foreach (TEntity child in Data.GetChildren())
            {
                if (child is IOptional)
                {
                    bool wasSkipped = Data.Mode != null && Data.Mode.CheckIfSkipped(child.GetType());
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
