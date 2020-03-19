using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Innoactive.Creator.Core.Attributes;
using Innoactive.Creator.Core.Conditions;
using Innoactive.Creator.Core.Configuration;
using Innoactive.Creator.Core.Configuration.Modes;
using Innoactive.Creator.Core.EntityOwners;
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

            [DataMember]
            [DrawingPriority(1)]
            public string Description { get; set; }

            [DataMember]
            public IBehaviorCollection Behaviors { get; set; }

            [DataMember]
            public ITransitionCollection Transitions { get; set; }

            public override IEnumerable<IStepChild> GetChildren()
            {
                return new List<IStepChild>
                {
                    Behaviors,
                    Transitions
                };
            }

            public IStepChild Current { get; set; }
            public IMode Mode { get; set; }
        }

        private class ActiveProcess : IStageProcess<EntityData>
        {
            public void Start(EntityData data)
            {
            }

            public IEnumerator Update(EntityData data)
            {
                while (data.Transitions.Data.Transitions.Any(transition => transition.IsCompleted) == false)
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

        [DataMember]
        public StepMetadata StepMetadata { get; set; }

        private readonly IProcess<EntityData> process = new CompositeProcess<EntityData>()
            .Add(new FoldedLifeCycleProcess<EntityData, IStepChild>())
            .Add(new ActiveOnlyProcess<EntityData>(new ActiveProcess()));

        protected override IProcess<EntityData> Process
        {
            get
            {
                return process;
            }
        }

        private readonly IConfigurator<EntityData> configurator = new BaseConfigurator<EntityData>().Add(new FoldedLifeCycleConfigurator<EntityData, IStepChild>());

        protected override IConfigurator<EntityData> Configurator
        {
            get
            {
                return configurator;
            }
        }

        IStepData IDataOwner<IStepData>.Data
        {
            get
            {
                return Data;
            }
        }

        protected Step() : this(null)
        {
        }

        public Step(string name)
        {
            StepMetadata = new StepMetadata();
            Data = new EntityData()
            {
                Transitions = new TransitionCollection(),
                Behaviors = new BehaviorCollection(),
                Name = name
            };

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
