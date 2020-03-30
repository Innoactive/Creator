using System.Collections;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Behaviors;
using Innoactive.Creator.Core.Configuration.Modes;
using UnityEngine;

namespace Innoactive.Creator.Tests.Utils.Mocks
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

        private class HasExecutionStageProcess : IStageProcess<EntityData>
        {

            private BehaviorExecutionStages executionStages;

            public HasExecutionStageProcess(BehaviorExecutionStages executionStages)
            {
                this.executionStages = executionStages;
            }

            public void Start(EntityData data)
            {

            }

            public IEnumerator Update(EntityData data)
            {
                if ((data.ExecutionStages & executionStages) > 0)
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

        public ActivationStageBehaviorMock(BehaviorExecutionStages executionStages, string name = "Activation Stage Mock")
        {
            Data = new EntityData
            {
                ExecutionStages = executionStages,
                Name = name
            };
        }

        private readonly IProcess<EntityData> process = new Process<EntityData>(new HasExecutionStageProcess(BehaviorExecutionStages.Activation), new EmptyStageProcess<EntityData>(), new HasExecutionStageProcess(BehaviorExecutionStages.Deactivation));

        protected override IProcess<EntityData> Process
        {
            get
            {
                return process;
            }
        }
    }
}
