using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Innoactive.Hub.Training.EntityOwners;
using Innoactive.Hub.Training.Configuration.Modes;

namespace Innoactive.Hub.Training.Behaviors
{
    /// <summary>
    /// Utility behavior to indicate that wrapped <see cref="Behavior"/> prevents its step from finishing the activation or deactivation until complete.
    /// </summary>
    [DataContract(IsReference = true)]
    [Obsolete("This class has multiple issues. It does not work with loops. Implement `IBackgroundBehaviorData` in the `EntityData` class of your behavior instead.")]
    public class NonblockingWrapperBehavior : Behavior<NonblockingWrapperBehavior.EntityData>
    {
        [DataContract(IsReference = true)]
        public class EntityData : EntityCollectionData<IBehavior>, IBehaviorData, IModeData
        {
            /// <summary>
            /// Does wrapped behavior block step activation?
            /// </summary>
            [DataMember]
            public bool IsBlocking { get; set; }

            /// <summary>
            /// Wrapped behavior.
            /// </summary>
            [DataMember]
            public IBehavior Behavior { get; set; }

            public override IEnumerable<IBehavior> GetChildren()
            {
                return new[] { Behavior };
            }

            public string Name { get; set; }

            public IMode Mode { get; set; }
        }

        private class ActivatingProcess : IStageProcess<EntityData>
        {
            public void Start(EntityData data)
            {
                if (data.Mode.CheckIfSkipped(data.Behavior.GetType()) == false)
                {
                    data.Behavior.LifeCycle.Activate();
                }
            }

            public IEnumerator Update(EntityData data)
            {
                while (data.IsBlocking && data.Behavior.LifeCycle.Stage == Stage.Activating)
                {
                    yield return null;
                }
            }

            public void End(EntityData data)
            {
            }

            public void FastForward(EntityData data)
            {
                if (data.Behavior.LifeCycle.Stage == Stage.Activating)
                {
                    data.Behavior.LifeCycle.MarkToFastForwardStage(Stage.Activating);
                }
            }
        }

        private class ActiveProcess : IStageProcess<EntityData>
        {
            public void Start(EntityData data)
            {
            }

            public IEnumerator Update(EntityData data)
            {
                yield break;
            }

            public void End(EntityData data)
            {
            }

            public void FastForward(EntityData data)
            {
                if (data.Behavior.LifeCycle.Stage == Stage.Activating)
                {
                    data.Behavior.LifeCycle.MarkToFastForwardStage(Stage.Activating);
                }
            }
        }

        private class DeactivatingProcess : IStageProcess<EntityData>
        {
            public void Start(EntityData data)
            {
                if (data.Behavior.LifeCycle.Stage == Stage.Activating || data.Behavior.LifeCycle.Stage == Stage.Active)
                {
                    data.Behavior.LifeCycle.Deactivate();
                }
            }

            public IEnumerator Update(EntityData data)
            {
                while (data.IsBlocking && data.Behavior.LifeCycle.Stage != Stage.Inactive)
                {
                    yield return null;
                }
            }

            public void End(EntityData data)
            {
            }

            public void FastForward(EntityData data)
            {
                if (data.Behavior.LifeCycle.Stage != Stage.Inactive)
                {
                    data.Behavior.LifeCycle.MarkToFastForward();
                }
            }
        }

        private class EntityConfigurator : IConfigurator<EntityData>
        {
            public void Configure(EntityData data, IMode mode, Stage stage)
            {
                if (stage == Stage.Inactive)
                {
                    return;
                }

                if (data.Behavior is IOptional == false)
                {
                    return;
                }

                bool wasSkipped = data.Mode.CheckIfSkipped(data.Behavior.GetType());
                bool isSkipped = mode.CheckIfSkipped(data.Behavior.GetType());

                if (wasSkipped == isSkipped)
                {
                    return;
                }

                if (isSkipped)
                {
                    if (data.Behavior.LifeCycle.Stage != Stage.Inactive && mode.CheckIfSkipped(data.Behavior.GetType()))
                    {
                        data.Behavior.LifeCycle.MarkToFastForward();

                        if (data.Behavior.LifeCycle.Stage == Stage.Active)
                        {
                            data.Behavior.LifeCycle.Deactivate();
                        }
                    }
                }
                else
                {
                    if (stage == Stage.Deactivating)
                    {
                        data.Behavior.LifeCycle.MarkToFastForwardStage(Stage.Activating);
                        data.Behavior.LifeCycle.MarkToFastForwardStage(Stage.Active);
                    }

                    data.Behavior.LifeCycle.Activate();
                }
            }
        }

        protected NonblockingWrapperBehavior() : this(null, false)
        {
        }

        public NonblockingWrapperBehavior(IBehavior behavior, bool isBlocking, string name = "")
        {
            Data = new EntityData();
            Data.Behavior = behavior;
            Data.IsBlocking = isBlocking;
            Data.Name = name;
        }

        private readonly IProcess<EntityData> process = new Process<EntityData>(new ActivatingProcess(), new ActiveProcess(), new DeactivatingProcess());

        protected override IProcess<EntityData> Process
        {
            get
            {
                return process;
            }
        }

        private readonly IConfigurator<EntityData> configurator = new BaseConfigurator<EntityData>().Add(new EntityConfigurator());

        protected override IConfigurator<EntityData> Configurator
        {
            get
            {
                return configurator;
            }
        }
    }
}
