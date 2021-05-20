using VPG.Core;
using VPG.Core.Conditions;
using System.Runtime.Serialization;

namespace VPG.Tests.Utils.Mocks
{
    /// <summary>
    /// Helper condition for testing that allows explicitly marking a condition as completed
    /// </summary>
    [DataContract(IsReference = true)]
    public class EndlessConditionMock : Condition<EndlessConditionMock.EntityData>
    {
        [DataContract(IsReference = true)]
        public class EntityData : IConditionData
        {
            [DataMember]
            public bool IsCompleted { get; set; }

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
