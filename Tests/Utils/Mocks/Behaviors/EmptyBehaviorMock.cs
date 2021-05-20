using VPG.Core;
using VPG.Core.Behaviors;

namespace VPG.Tests.Utils.Mocks
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
