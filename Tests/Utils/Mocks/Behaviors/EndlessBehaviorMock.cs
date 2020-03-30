using System.Collections;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Behaviors;

namespace Innoactive.Creator.Tests.Utils.Mocks
{
    /// <summary>
    /// Helper behavior for testing that allows explicitly triggering finishing Activation / Deactivation
    /// </summary>
    public class EndlessBehaviorMock : Behavior<EndlessBehaviorMock.EntityData>
    {
        public class EntityData : IBackgroundBehaviorData
        {
            public Metadata Metadata { get; set; }
            public string Name { get; set; }
            public bool IsBlocking { get; set; }
        }

        private class LoopProcess : IStageProcess<EntityData>
        {
            public void Start(EntityData data)
            {
            }

            public IEnumerator Update(EntityData data)
            {
                int endlessLoopProtection = 1000000;
                while (endlessLoopProtection > 0)
                {
                    endlessLoopProtection++;
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

        private readonly IProcess<EntityData> process = new Process<EntityData>(new LoopProcess(), new EmptyStageProcess<EntityData>(), new LoopProcess());

        protected override IProcess<EntityData> Process
        {
            get
            {
                return process;
            }
        }

        public EndlessBehaviorMock(bool isBlocking = true)
        {
            Data = new EntityData()
            {
                IsBlocking = isBlocking
            };
        }
    }
}
