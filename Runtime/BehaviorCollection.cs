using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Innoactive.Creator.Core.Attributes;
using Innoactive.Creator.Core.Behaviors;
using Innoactive.Creator.Core.Configuration;
using Innoactive.Creator.Core.Configuration.Modes;
using Innoactive.Creator.Core.EntityOwners;

namespace Innoactive.Creator.Core
{
    [DataContract(IsReference = true)]
    public class BehaviorCollection : Entity<BehaviorCollection.EntityData>, IBehaviorCollection
    {
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

        private readonly IProcess<EntityData> process = new ParallelLifeCycleProcess<EntityData, IBehavior>();

        protected override IProcess<EntityData> Process
        {
            get
            {
                return process;
            }
        }

        private readonly IConfigurator<EntityData> configurator = new BaseConfigurator<EntityData>().Add(new ParallelLifeCycleConfigurator<EntityData, IBehavior>());

        protected override IConfigurator<EntityData> Configurator
        {
            get
            {
                return configurator;
            }
        }

        IBehaviorCollectionData IDataOwner<IBehaviorCollectionData>.Data
        {
            get
            {
                return Data;
            }
        }

        public BehaviorCollection()
        {
            Data = new EntityData { Behaviors = new List<IBehavior>() };
        }
    }
}
