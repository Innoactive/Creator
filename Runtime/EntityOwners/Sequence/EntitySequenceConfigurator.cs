using Innoactive.Creator.Core.Configuration.Modes;

namespace Innoactive.Creator.Core.EntityOwners
{
    public class EntitySequenceConfigurator<TData, TEntity> : IConfigurator<TData> where TData : IEntitySequenceData<TEntity> where TEntity : IEntity
    {
        public void Configure(TData data, IMode mode, Stage stage)
        {
            if (data.Current == null)
            {
                return;
            }

            if (data.Current is IOptional
                && mode.CheckIfSkipped(data.Current.GetType())
                && data.Current.LifeCycle.Stage != Stage.Inactive)
            {
                data.Current.LifeCycle.MarkToFastForward();
            }
        }
    }
}
