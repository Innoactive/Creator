using Innoactive.Creator.Core.Configuration.Modes;

namespace Innoactive.Creator.Core.EntityOwners
{
    /// <summary>
    /// A configurator for a sequence of entities.
    /// </summary>
    public class SequenceConfigurator<TEntity> : Configurator<IEntitySequenceData<TEntity>> where TEntity : IEntity
    {
        public SequenceConfigurator(IEntitySequenceData<TEntity> data) : base(data)
        {
        }

        ///<inheritdoc />
        public override void Configure(IMode mode, Stage stage)
        {
            if (Data.Current == null)
            {
                return;
            }

            if (Data.Current is IOptional
                && mode.CheckIfSkipped(Data.Current.GetType())
                && Data.Current.LifeCycle.Stage != Stage.Inactive)
            {
                Data.Current.LifeCycle.MarkToFastForward();
            }
        }
    }
}
