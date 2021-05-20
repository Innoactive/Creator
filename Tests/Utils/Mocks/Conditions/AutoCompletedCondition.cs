using System.Runtime.Serialization;
using VPG.Core;
using VPG.Core.Conditions;

namespace VPG.Tests.Utils.Mocks
{
    [DataContract(IsReference = true)]
    public class AutoCompletedCondition : Condition<AutoCompletedCondition.EntityData>
    {
        [DataContract(IsReference = true)]
        public class EntityData : IConditionData
        {
            public bool IsCompleted { get; set; }

            public string Name { get; set; }

            public Metadata Metadata { get; set; }
        }

        private class ActiveProcess : InstantProcess<EntityData>
        {
            public ActiveProcess(EntityData data) : base(data)
            {
            }

            public override void Start()
            {
                Data.IsCompleted = true;
            }
        }

        public override IProcess GetActiveProcess()
        {
            return new ActiveProcess(Data);
        }
    }
}
