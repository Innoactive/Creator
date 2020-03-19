using System.Runtime.Serialization;
using Innoactive.Hub.Training;
using Innoactive.Hub.Training.Conditions;

namespace Innoactive.Creator.Core.Tests.Utils.Mocks
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

        private class ActiveProcess : InstantStageProcess<EntityData>
        {
            public override void Start(EntityData data)
            {
                data.IsCompleted = false;
            }
        }

        private readonly IProcess<EntityData> process = new ActiveOnlyProcess<EntityData>(new ActiveProcess());

        protected override IProcess<EntityData> Process
        {
            get
            {
                return process;
            }
        }

        private readonly IAutocompleter<EntityData> autocompleter = new BaseAutocompleter<EntityData>();

        protected override IAutocompleter<EntityData> Autocompleter
        {
            get
            {
                return autocompleter;
            }
        }

        public EndlessConditionMock()
        {
            Data = new EntityData();
        }
    }
}
