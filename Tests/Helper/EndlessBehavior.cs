#if UNITY_EDITOR

using System.Collections;
using Innoactive.Hub.Training;
using Innoactive.Hub.Training.Behaviors;

namespace Innoactive.Hub.Unity.Tests.Training
{
    /// <summary>
    /// Helper behavior for testing that allows explicitly triggering finishing Activation / Deactivation
    /// </summary>
    public class EndlessBehavior : Behavior<EndlessBehavior.EntityData>
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

        public EndlessBehavior(bool isBlocking = true)
        {
            Data = new EntityData()
            {
                IsBlocking = isBlocking
            };
        }
    }
}
#endif
