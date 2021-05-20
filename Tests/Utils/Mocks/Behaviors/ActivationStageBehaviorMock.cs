using System.Collections;
using VPG.Core;
using VPG.Core.Behaviors;
using VPG.Core.Configuration.Modes;
using UnityEngine;

namespace VPG.Tests.Utils.Mocks
{
    /// <summary>
    /// Helper behavior for testing which has an activation state.
    /// </summary>
    public class ActivationStageBehaviorMock : Behavior<ActivationStageBehaviorMock.EntityData>, IOptional
    {
        public class EntityData : IBehaviorData
        {
            public BehaviorExecutionStages ExecutionStages { get; set; }

            public AudioSource AudioPlayer { get; set; }

            public Metadata Metadata { get; set; }
            public string Name { get; set; }
        }

        private class HasExecutionStageProcess : Process<EntityData>
        {
            private BehaviorExecutionStages executionStages;

            public HasExecutionStageProcess(BehaviorExecutionStages executionStages, EntityData data) : base(data)
            {
                this.executionStages = executionStages;
            }

            public override void Start()
            {
            }

            public override IEnumerator Update()
            {
                if ((Data.ExecutionStages & executionStages) > 0)
                {
                    yield return null;
                }
            }

            public override void End()
            {
            }

            public override void FastForward()
            {
            }
        }

        public ActivationStageBehaviorMock(BehaviorExecutionStages executionStages, string name = "Activation Stage Mock")
        {
            Data.ExecutionStages = executionStages;
            Data.Name = name;
        }

        public override IProcess GetActivatingProcess()
        {
            return new HasExecutionStageProcess(BehaviorExecutionStages.Activation, Data);
        }

        public override IProcess GetDeactivatingProcess()
        {
            return new HasExecutionStageProcess(BehaviorExecutionStages.Deactivation, Data);
        }
    }
}
