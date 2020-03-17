using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Innoactive.Creator.Core.Attributes;
using Innoactive.Creator.Core.Configuration;
using Innoactive.Creator.Core.Configuration.Modes;
using Innoactive.Creator.Core.EntityOwners;

namespace Innoactive.Creator.Core
{
    [DataContract(IsReference = true)]
    public class TransitionCollection : Entity<TransitionCollection.EntityData>, ITransitionCollection
    {
        [DataContract(IsReference = true)]
        public class EntityData : EntityCollectionData<ITransition>, ITransitionCollectionData
        {
            [DataMember]
            [DisplayName("Transitions"), Separated, Foldable, ListOf(typeof(FoldableAttribute), typeof(DeletableAttribute), typeof(SeparatedAttribute)), ExtendableList]
            public virtual IList<ITransition> Transitions { get; set; }

            public override IEnumerable<ITransition> GetChildren()
            {
                return Transitions.ToArray();
            }

            public IMode Mode { get; set; }
        }

        private class ActiveProcess : IStageProcess<EntityData>
        {
            public void Start(EntityData data)
            {
            }

            public IEnumerator Update(EntityData data)
            {
                while (data.Transitions.All(transition => transition.IsCompleted == false))
                {
                    yield return null;
                }
            }

            public void End(EntityData data)
            {
            }

            public void FastForward(EntityData data)
            {
            }
        }

        private readonly IProcess<EntityData> process = new CompositeProcess<EntityData>()
            .Add(new ParallelLifeCycleProcess<EntityData, ITransition>())
            .Add(new Process<EntityData>(new EmptyStageProcess<EntityData>(), new ActiveProcess(), new EmptyStageProcess<EntityData>()));

        protected override IProcess<EntityData> Process
        {
            get
            {
                return process;
            }
        }

        private readonly IConfigurator<EntityData> configurator = new BaseConfigurator<EntityData>().Add(new ParallelLifeCycleConfigurator<EntityData, ITransition>());
        private ITransitionCollectionData data;

        protected override IConfigurator<EntityData> Configurator
        {
            get
            {
                return configurator;
            }
        }

        ITransitionCollectionData IDataOwner<ITransitionCollectionData>.Data
        {
            get
            {
                return Data;
            }
        }

        public TransitionCollection()
        {
            Data = new EntityData()
            {
                Transitions = new List<ITransition>(),
            };
        }
    }
}
