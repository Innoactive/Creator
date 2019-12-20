#if UNITY_EDITOR

using Innoactive.Hub.Training;
using Innoactive.Hub.Training.Behaviors;

namespace Innoactive.Hub.Unity.Tests.Training
{
    /// <summary>
    /// Helper behavior for testing that does nothing
    /// </summary>
    public class EmptyBehavior : Behavior<EmptyBehavior.EntityData>
    {
        public class EntityData : IBehaviorData
        {
            public Metadata Metadata { get; set; }
            public string Name { get; set; }
        }

        public EmptyBehavior()
        {
            Data = new EntityData();
        }

        private readonly IProcess<EntityData> process = new Process<EntityData>(new EmptyStageProcess<EntityData>(), new EmptyStageProcess<EntityData>(), new EmptyStageProcess<EntityData>());

        protected override IProcess<EntityData> Process
        {
            get
            {
                return process;
            }
        }
    }
}
#endif
