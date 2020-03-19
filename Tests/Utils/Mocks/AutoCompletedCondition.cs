using System.Runtime.Serialization;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Conditions;

namespace Innoactive.Creator.Tests.Utils.Mocks
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

        private class ActiveProcess : InstantStageProcess<EntityData>
        {
            public override void Start(EntityData data)
            {
                data.IsCompleted = true;
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

        public AutoCompletedCondition()
        {
            Data = new EntityData();
        }
    }
}
