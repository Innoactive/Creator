using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using VPG.Creator.Core.Attributes;
using VPG.Creator.Core.Behaviors;
using VPG.Creator.Core.Configuration.Modes;
using VPG.Creator.Core.EntityOwners;
using VPG.Creator.Core.EntityOwners.ParallelEntityCollection;

namespace VPG.Creator.Core
{
    /// <summary>
    /// A collection of <see cref="IBehavior"/>s of a <see cref="IStep"/>.
    /// </summary>
    [DataContract(IsReference = true)]
    public class BehaviorCollection : Entity<BehaviorCollection.EntityData>, IBehaviorCollection
    {
        /// <summary>
        /// The data class for <see cref="IBehavior"/> collections.
        /// </summary>
        [DataContract(IsReference = true)]
        public class EntityData : EntityCollectionData<IBehavior>, IBehaviorCollectionData
        {
            /// <summary>
            /// List of all <see cref="IBehavior"/>s added.
            /// </summary>
            [DataMember]
            [DisplayName(""), ReorderableListOf(typeof(FoldableAttribute), typeof(DeletableAttribute), typeof(DrawIsBlockingToggleAttribute), typeof(HelpAttribute)), ExtendableList]
            public virtual IList<IBehavior> Behaviors { get; set; }

            /// <summary>
            /// Returns a list of all <see cref="IBehavior"/>s added.
            /// </summary>
            public override IEnumerable<IBehavior> GetChildren()
            {
                return Behaviors.ToList();
            }

            /// <summary>
            /// Reference to <see cref="IBehavior"/>'s current mode.
            /// </summary>
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
