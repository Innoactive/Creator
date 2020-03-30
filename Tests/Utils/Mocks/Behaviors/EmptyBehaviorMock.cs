using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Behaviors;

namespace Innoactive.Creator.Tests.Utils.Mocks
{
    /// <summary>
    /// Helper behavior for testing that does nothing
    /// </summary>
    public class EmptyBehaviorMock : Behavior<EmptyBehaviorMock.EntityData>
    {
        public class EntityData : IBehaviorData
        {
            public Metadata Metadata { get; set; }
            public string Name { get; set; }
        }

        public EmptyBehaviorMock()
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
