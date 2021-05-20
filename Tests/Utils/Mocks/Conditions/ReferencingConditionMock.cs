using System.Collections.Generic;
using System.Runtime.Serialization;
using VPG.Core;
using VPG.Core.Conditions;
using VPG.Core.Properties;
using VPG.Core.RestrictiveEnvironment;
using VPG.Core.SceneObjects;

namespace VPG.Tests.Utils.Mocks
{
    /// <summary>
    /// Helper condition for testing that allows explicitly marking a condition as completed.
    /// It can reference a <see cref="PropertyMock"/>.
    /// </summary>
    public class ReferencingConditionMock : Condition<ReferencingConditionMock.EntityData>
    {
        [DataContract(IsReference = true)]
        public class EntityData : IConditionData
        {
            [DataMember]
            public bool IsCompleted { get; set; }

            [DataMember]
            public ScenePropertyReference<PropertyMock> PropertyMock { get; set; }

            [DataMember]
            public string Name { get; set; }

            [DataMember]
            public Metadata Metadata { get; set; }
        }

        private class ActiveProcess : InstantProcess<EntityData>
        {
            public override void Start()
            {
                Data.IsCompleted = false;
            }

            public ActiveProcess(EntityData data) : base(data)
            {
            }
        }

        public override IProcess GetActiveProcess()
        {
            return new ActiveProcess(Data);
        }
    }
}
