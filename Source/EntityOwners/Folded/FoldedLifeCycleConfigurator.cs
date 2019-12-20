using System;
using Innoactive.Hub.Training.Configuration.Modes;

namespace Innoactive.Hub.Training.EntityOwners
{
    public class FoldedLifeCycleConfigurator<TData, TEntity> : IConfigurator<TData> where TEntity : IEntity where TData : IEntitySequenceData<TEntity>
    {
        public void Configure(TData data, IMode mode, Stage stage)
        {
            if (stage == Stage.Inactive)
            {
                return;
            }

            try
            {
                if (data.Current is IOptional == false)
                {
                    return;
                }
            }
            catch (InvalidOperationException)
            {
                return;
            }

            if (mode.CheckIfSkipped(data.Current.GetType()))
            {
                data.Current.LifeCycle.MarkToFastForwardStage(stage);
            }
        }
    }
}
