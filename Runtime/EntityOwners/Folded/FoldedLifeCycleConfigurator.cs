using System;
using VPG.Creator.Core.Configuration.Modes;

namespace VPG.Creator.Core.EntityOwners
{
    /// <summary>
    /// A configurator for a sequence of entities.
    /// </summary>
    public class FoldedLifeCycleConfigurator<TEntity> : Configurator<IEntitySequenceData<TEntity>> where TEntity : IEntity
    {
        public FoldedLifeCycleConfigurator(IEntitySequenceData<TEntity> data) : base(data)
        {
        }

        /// <inheritdoc />
        public override void Configure(IMode mode, Stage stage)
        {
            if (stage == Stage.Inactive)
            {
                return;
            }

            try
            {
                if (Data.Current is IOptional == false)
                {
                    return;
                }
            }
            catch (InvalidOperationException)
            {
                return;
            }

            if (mode.CheckIfSkipped(Data.Current.GetType()))
            {
                Data.Current.LifeCycle.MarkToFastForwardStage(stage);
            }
        }
    }
}
