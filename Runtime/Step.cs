﻿using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Innoactive.Creator.Core.Attributes;
using Innoactive.Creator.Core.Behaviors;
using Innoactive.Creator.Core.Configuration;
using Innoactive.Creator.Core.Configuration.Modes;
using Innoactive.Creator.Core.EntityOwners;
using Innoactive.Creator.Core.EntityOwners.FoldedEntityCollection;
using Innoactive.Creator.Core.RestrictiveEnvironment;
using Innoactive.Creator.Core.SceneObjects;
using Innoactive.Creator.Core.Tabs;
using Innoactive.Creator.Core.Utils.Logging;
using Innoactive.Creator.Unity;

namespace Innoactive.Creator.Core
{
    /// <summary>
    /// An implementation of <see cref="IStep"/> interface.
    /// </summary>
    [DataContract(IsReference = true)]
    public class Step : Entity<Step.EntityData>, IStep
    {
        public class EntityData : EntityCollectionData<IStepChild>, IStepData, ILockableStepData
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

            [DataMember]
            private ITabsGroup Tabs { get; set; }

            ///<inheritdoc />
            [DataMember]
            [HideInTrainingInspector]
            public IBehaviorCollection Behaviors { get; set; }

            ///<inheritdoc />
            [DataMember]
            [HideInTrainingInspector]
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

            ///<inheritdoc />
            [HideInTrainingInspector]
            public IEnumerable<LockablePropertyReference> ToUnlock { get; set; } = new List<LockablePropertyReference>();

            public EntityData()
            {
                Tabs = new TabsGroup(
                    new DynamicTab(new GUIContent("Behaviors"), () => Behaviors, value => Behaviors = (IBehaviorCollection)value),
                    new DynamicTab(new GUIContent("Transitions"), () => Transitions, value => Transitions = (ITransitionCollection)value),
                    new LockablePropertyTab(new GUIContent("Unlocked Objects"), this)
                );
            }
        }

        private class UnlockProcess : Process<EntityData>
        {
            private readonly IEnumerable<LockablePropertyData> toUnlock;

            public UnlockProcess(EntityData data) : base(data)
            {
                toUnlock = Data.ToUnlock.Select(reference => new LockablePropertyData(reference.GetProperty())).ToList();
            }

            ///<inheritdoc />
            public override void Start()
            {
                RuntimeConfigurator.Configuration.StepLockHandling.Unlock(Data, toUnlock);
            }

            ///<inheritdoc />
            public override IEnumerator Update()
            {
                yield return null;
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

        private class LockProcess : Process<EntityData>
        {
            private readonly IEnumerable<LockablePropertyData> toUnlock;

            public LockProcess(EntityData data) : base(data)
            {
                toUnlock = Data.ToUnlock.Select(reference => new LockablePropertyData(reference.GetProperty())).ToList();
            }

            ///<inheritdoc />
            public override void Start()
            {
            }

            ///<inheritdoc />
            public override IEnumerator Update()
            {
                yield return null;
            }

            ///<inheritdoc />
            public override void End()
            {
                RuntimeConfigurator.Configuration.StepLockHandling.Lock(Data, toUnlock);
            }

            ///<inheritdoc />
            public override void FastForward()
            {
            }
        }

        private class ActiveProcess : Process<EntityData>
        {
            private readonly IEnumerable<LockablePropertyData> toUnlock;

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
            return new CompositeProcess(new FoldedActivatingProcess<IStepChild>(Data), new UnlockProcess(Data));
        }

        ///<inheritdoc />
        public override IProcess GetActiveProcess()
        {
            return new CompositeProcess(new FoldedActiveProcess<IStepChild>(Data), new ActiveProcess(Data));
        }

        ///<inheritdoc />
        public override IProcess GetDeactivatingProcess()
        {
            return new CompositeProcess(new FoldedDeactivatingProcess<IStepChild>(Data), new LockProcess(Data));
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

            if (LifeCycleLoggingConfig.Instance.LogSteps)
            {
                LifeCycle.StageChanged += (sender, args) =>
                {
                    Debug.LogFormat("{0}<b>Step</b> <i>'{1}'</i> is <b>{2}</b>.\n", ConsoleUtils.GetTabs(), Data.Name, LifeCycle.Stage);
                };
            }
        }
    }
}
