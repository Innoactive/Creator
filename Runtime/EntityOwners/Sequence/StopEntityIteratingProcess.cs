using System.Collections;

namespace VPG.Creator.Core.EntityOwners
{
    /// <summary>
    /// A process that stops activation of a current entity in the entity sequence.
    /// </summary>
    public class StopEntityIteratingProcess<TEntity> : Process<IEntitySequenceData<TEntity>> where TEntity : IEntity
    {
        ///<inheritdoc />
        public override void Start()
        {
        }

        ///<inheritdoc />
        public override IEnumerator Update()
        {
            if (Data.Current == null)
            {
                yield break;
            }

            if (Data.Current.LifeCycle.Stage == Stage.Activating || Data.Current.LifeCycle.Stage == Stage.Active)
            {
                Data.Current.LifeCycle.Deactivate();
            }

            while (Data.Current.LifeCycle.Stage != Stage.Inactive)
            {
                yield return null;
            }
        }

        ///<inheritdoc />
        public override void End()
        {
            Data.Current = default;
        }

        ///<inheritdoc />
        public override void FastForward()
        {
            Data.Current?.LifeCycle.MarkToFastForward();
        }

        public StopEntityIteratingProcess(IEntitySequenceData<TEntity> data) : base(data)
        {
        }
    }
}
