using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Behaviors;

namespace Innoactive.Creator.Tests.Utils.Mocks
{
    /// <summary>
    /// Helper behavior for testing which just has a float value.
    /// </summary>
    public class ValueBehaviorMock : Behavior<ValueBehaviorMock.EntityData>
    {
        public class EntityData : IBehaviorData
        {
            /// <summary>
            /// Value which can be set.
            /// </summary>
            public float Value { get; set; }

            public Metadata Metadata { get; set; }
            public string Name { get; set; }
        }

        public ValueBehaviorMock(float value)
        {
            Data.Value = value;
        }
    }
}
