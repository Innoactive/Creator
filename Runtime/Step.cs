using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Innoactive.Creator.Core.Attributes;
using Innoactive.Creator.Core.Configuration;
using Innoactive.Creator.Core.Configuration.Modes;
using Innoactive.Creator.Core.EntityOwners;
using Innoactive.Creator.Core.EntityOwners.FoldedEntityCollection;
using Innoactive.Creator.Unity;

namespace Innoactive.Creator.Core
{
    /// <summary>
    /// An implementation of <see cref="IStep"/> interface.
    /// </summary>
    [DataContract(IsReference = true)]
    public class Step : Entity<Step.EntityData>, IStep
    {
        public class EntityData : EntityCollectionData<IStepChild>, IStepData
        {
            ///<inheritdoc />
            [DataMember]
            [DrawingPriority(0)]
            [HideInTrainingInspector]
            public string Name { get; set; }

            ///<inheritdoc />
            [DataMember]
            [DrawingPriority(1)]
            public string Description { get; set; }

            ///<inheritdoc />
            [DataMember]
            public IBehaviorCollection Behaviors { get; set; }

            ///<inheritdoc />
            [DataMember]
            public ITransitionCollection Transitions { get; set; }

            ///<inheritdoc />
            public override IEnumerable<IStepChild> GetChildren()
            {
                return new List<IStepChild>
                {
                    Behaviors,
                    Transitions
                };
            }

            ///<inheritdoc />
            public IStepChild Current { get; set; }

            ///<inheritdoc />
            public IMode Mode { get; set; }
        }

        private class ActiveProcess : Process<EntityData>
        {
            public ActiveProcess(EntityData data) : base(data)
            {
            }

            ///<inheritdoc />
            public override void Start()
            {
            }

            ///<inheritdoc />
            public override IEnumerator Update()
            {
                while (Data.Transitions.Data.Transitions.Any(transition => transition.IsCompleted) == false)
                {
                    yield return null;
                }
            }

            ///<inheritdoc />
            public override void End()
            {
            }

            ///<inheritdoc />
            public override void FastForward()
            {
            }
        }

        ///<inheritdoc />
        [DataMember]
        public StepMetadata StepMetadata { get; set; }

        ///<inheritdoc />
        public override IProcess GetActivatingProcess()
        {
            return new FoldedActivatingProcess<IStepChild>(Data);
        }

        ///<inheritdoc />
        public override IProcess GetActiveProcess()
        {
            return new CompositeProcess(new FoldedActiveProcess<IStepChild>(Data), new ActiveProcess(Data));
        }

        ///<inheritdoc />
        public override IProcess GetDeactivatingProcess()
        {
            return new FoldedDeactivatingProcess<IStepChild>(Data);
        }

        ///<inheritdoc />
        protected override IConfigurator GetConfigurator()
        {
            return new FoldedLifeCycleConfigurator<IStepChild>(Data);
        }

        ///<inheritdoc />
        IStepData IDataOwner<IStepData>.Data
        {
            get { return Data; }
        }

        protected Step() : this(null)
        {
        }

        public Step(string name)
        {
            StepMetadata = new StepMetadata();
            Data.Transitions = new TransitionCollection();
            Data.Behaviors = new BehaviorCollection();
            Data.Name = name;

            if (RuntimeConfigurator.Configuration.EntityStateLoggerConfig.LogSteps)
            {
                LifeCycle.StageChanged += (sender, args) =>
                {
                    RuntimeConfigurator.Configuration.EntityStateLogger.LogFormat(LogType.Log, "{0}<b>Step</b> <i>'{1}'</i> is <b>{2}</b>.\n", ConsoleUtils.GetTabs(), Data.Name, LifeCycle.Stage);
                };
            }
        }
    }
}
