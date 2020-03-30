using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Innoactive.Creator.Core.Attributes;
using Innoactive.Creator.Core.Behaviors;
using Innoactive.Creator.Core.Configuration.Modes;
using Innoactive.Creator.Core.EntityOwners;
using Innoactive.Creator.Core.EntityOwners.ParallelEntityCollection;

namespace Innoactive.Creator.Core
{
    /// <summary>
    /// A collection of behaviors of a step.
    /// </summary>
    [DataContract(IsReference = true)]
    public class BehaviorCollection : Entity<BehaviorCollection.EntityData>, IBehaviorCollection
    {
        /// <summary>
        /// The data class for behavior collections.
        /// </summary>
        [DataContract(IsReference = true)]
        public class EntityData : EntityCollectionData<IBehavior>, IBehaviorCollectionData
        {
            [DataMember]
            [DisplayName("Behaviors"), Separated, Foldable, ListOf(typeof(FoldableAttribute), typeof(DeletableAttribute), typeof(SeparatedAttribute), typeof(DrawIsBlockingToggleAttribute)), ExtendableList]
            public virtual IList<IBehavior> Behaviors { get; set; }

            public override IEnumerable<IBehavior> GetChildren()
            {
                return Behaviors.ToList();
            }

            public IMode Mode { get; set; }
        }

        /// <inheritdoc />
        public override IProcess GetActivatingProcess()
        {
            return new ParallelActivatingProcess<EntityData>(Data);
        }

        /// <inheritdoc />
        public override IProcess GetActiveProcess()
        {
            return new ParallelActiveProcess<EntityData>(Data);
        }

        /// <inheritdoc />
        public override IProcess GetDeactivatingProcess()
        {
            return new ParallelDeactivatingProcess<EntityData>(Data);
        }

        /// <inheritdoc />
        protected override IConfigurator GetConfigurator()
        {
            return new ParallelConfigurator<IBehavior>(Data);
        }

        /// <inheritdoc />
        IBehaviorCollectionData IDataOwner<IBehaviorCollectionData>.Data
        {
            get { return Data; }
        }

        public BehaviorCollection()
        {
            Data.Behaviors = new List<IBehavior>();
        }
    }
}
