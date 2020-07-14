using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Innoactive.Creator.Core.EntityOwners.FoldedEntityCollection
{
    /// <summary>
    /// A process over entities' sequence which deactivates entities in an opposite order.
    /// </summary>
    internal class FoldedDeactivatingProcess<TEntity> : Process<IEntitySequenceDataWithMode<TEntity>> where TEntity : IEntity
    {
        private IEnumerator<TEntity> enumerator;

        public FoldedDeactivatingProcess(IEntitySequenceDataWithMode<TEntity> data) : base(data)
        {
        }

        /// <inheritdoc />
        public override void Start()
        {
            enumerator = Data.GetChildren().Reverse().GetEnumerator();
        }

        /// <inheritdoc />
        public override IEnumerator Update()
        {
            while (enumerator.MoveNext())
            {
                Data.Current = enumerator.Current;

                if (Data.Current == null)
                {
                    continue;
                }

                if (Data.Current.LifeCycle.Stage != Stage.Inactive)
                {
                    Data.Current.LifeCycle.Deactivate();
                }

                if (Data.Current.LifeCycle.Stage == Stage.Deactivating && Data.Mode.CheckIfSkipped(Data.Current.GetType()))
                {
                    Data.Current.LifeCycle.MarkToFastForwardStage(Stage.Deactivating);
                }

                while (Data.Current.LifeCycle.Stage != Stage.Inactive)
                {
                    yield return null;
                }
            }
        }

        /// <inheritdoc />
        public override void End()
        {
            enumerator = null;
        }

        /// <inheritdoc />
        public override void FastForward()
        {
            if (Equals(Data.Current, default))
            {
                if (enumerator.MoveNext())
                {
                    Data.Current = enumerator.Current;
                }
            }

            while (Equals(Data.Current, default) == false)
            {
                if (Data.Current == null)
                {
                    throw new NullReferenceException();
                }

                if (Data.Current.LifeCycle.Stage == Stage.Active)
                {
                    Data.Current.LifeCycle.Deactivate();
                }

                if (Data.Current.LifeCycle.Stage == Stage.Deactivating)
                {
                    Data.Current.LifeCycle.MarkToFastForwardStage(Stage.Deactivating);
                }

                Data.Current = enumerator.MoveNext() ? enumerator.Current : default;
            }
        }
    }
}
