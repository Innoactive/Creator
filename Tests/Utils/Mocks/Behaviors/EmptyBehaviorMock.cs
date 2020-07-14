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
    }
}
