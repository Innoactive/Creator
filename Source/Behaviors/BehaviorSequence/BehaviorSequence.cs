using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Innoactive.Hub.Training.EntityOwners;
using Innoactive.Hub.Training.Attributes;
using Innoactive.Hub.Training.Configuration.Modes;
using Newtonsoft.Json;

namespace Innoactive.Hub.Training.Behaviors
{
    /// <summary>
    /// A collection of behaviors that are activated and deactivated after each other.
    /// </summary>
    [DataContract(IsReference = true)]
    public class BehaviorSequence : Behavior<BehaviorSequence.EntityData>
    {
        [DisplayName("Behavior Sequence")]
        [DataContract(IsReference = true)]
        public class EntityData : EntityCollectionData<IBehavior>, IEntitySequenceData<IBehavior>, IBackgroundBehaviorData, IModeData
        {
            /// <summary>
            /// Are child behaviors activated only once or the collection is cycled.
            /// </summary>
            [DisplayName("Repeat")]
            [DataMember]
            public bool PlaysOnRepeat { get; set; }

            /// <summary>
            /// List of child behaviors.
            /// </summary>
            [DataMember]
            [DisplayName("Child behaviors")]
            [Foldable, ListOf(typeof(FoldableAttribute), typeof(DeletableAttribute)), ExtendableList]
            public List<IBehavior> Behaviors { get; set; }

            public override IEnumerable<IBehavior> GetChildren()
            {
                return Behaviors.ToList();
            }

            public IBehavior Current { get; set; }

            public string Name { get; set; }
            public IMode Mode { get; set; }

            /// <inheritdoc />
            public bool IsBlocking { get; set; }
        }

        private class IteratingProcess : EntityIteratingProcess<EntityData, IBehavior>
        {
            private IEnumerator<IBehavior> enumerator;

            public override void Start(EntityData data)
            {
                base.Start(data);
                enumerator = data.GetChildren().GetEnumerator();
            }

            protected override bool ShouldActivateCurrent(EntityData data)
            {
                return true;
            }

            protected override bool ShouldDeactivateCurrent(EntityData data)
            {
                return true;
            }

            protected override bool TryNext(out IBehavior entity)
            {
                if (enumerator == null || (enumerator.MoveNext() == false))
                {
                    entity = default(IBehavior);
                    return false;
                }
                else
                {
                    entity = enumerator.Current;
                    return true;
                }
            }
        }

        private class ActiveProcess : IStageProcess<EntityData>
        {
            private readonly IStageProcess<EntityData> childProcess = new IteratingProcess();

            public void Start(EntityData data)
            {
                childProcess.Start(data);
            }

            public IEnumerator Update(EntityData data)
            {
                if (data.PlaysOnRepeat == false)
                {
                    yield break;
                }

                int endlessLoopCheck = 0;

                while (endlessLoopCheck < 100000)
                {
                    IEnumerator update = childProcess.Update(data);

                    while (update.MoveNext())
                    {
                        yield return null;
                    }

                    childProcess.End(data);

                    childProcess.Start(data);

                    endlessLoopCheck++;
                }
            }

            public void End(EntityData data)
            {
            }

            public void FastForward(EntityData data)
            {
                childProcess.FastForward(data);
                childProcess.End(data);
            }
        }

        [JsonConstructor]
        public BehaviorSequence() : this(default(bool), new List<IBehavior>())
        {
        }

        public BehaviorSequence(bool playsOnRepeat, IList<IBehavior> behaviors, string name = "Sequence")
        {
            Data = new EntityData()
            {
                PlaysOnRepeat = playsOnRepeat,
                Behaviors = new List<IBehavior>(behaviors),
                Name = name,
                IsBlocking = true
            };
        }

        public BehaviorSequence(bool playsOnRepeat, IList<IBehavior> behaviors, bool isBlocking, string name = "Sequence") : this(playsOnRepeat, behaviors, name)
        {
            Data.IsBlocking = isBlocking;
        }

        private readonly IProcess<EntityData> process = new Process<EntityData>(new IteratingProcess(), new ActiveProcess(), new StopEntityIteratingProcess<EntityData, IBehavior>());

        protected override IProcess<EntityData> Process
        {
            get
            {
                return process;
            }
        }

        private readonly IConfigurator<EntityData> configurator = new BaseConfigurator<EntityData>().Add(new EntitySequenceConfigurator<EntityData, IBehavior>());

        protected override IConfigurator<EntityData> Configurator
        {
            get
            {
                return configurator;
            }
        }
    }
}
