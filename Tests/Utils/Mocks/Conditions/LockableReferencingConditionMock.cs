using System.Collections.Generic;
using System.Runtime.Serialization;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Conditions;
using Innoactive.Creator.Core.RestrictiveEnvironment;
using Innoactive.Creator.Core.SceneObjects;

namespace Innoactive.Creator.Tests.Utils.Mocks
{
    /// <summary>
    /// Helper condition for testing that allows explicitly marking a condition as completed.
    /// It can reference a <see cref="LockablePropertyMock"/>.
    /// </summary>
    public class LockableReferencingConditionMock : Condition<LockableReferencingConditionMock.EntityData>
    {
        public IEnumerable<LockablePropertyData> LockableProperties = null;

        [DataContract(IsReference = true)]
        public class EntityData : IConditionData
        {
            [DataMember]
            public bool IsCompleted { get; set; }

            [DataMember]
            public ScenePropertyReference<ILockablePropertyMock> LockablePropertyMock { get; set; }

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

        public override IEnumerable<LockablePropertyData> GetLockableProperties()
        {
            if (LockableProperties == null)
            {
                return base.GetLockableProperties();
            }

            return LockableProperties;
        }

        public override IProcess GetActiveProcess()
        {
            return new ActiveProcess(Data);
        }
    }
}
