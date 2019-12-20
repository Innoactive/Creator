#if UNITY_EDITOR

using Innoactive.Hub.Training;
using Innoactive.Hub.Training.Conditions;

namespace Innoactive.Hub.Unity.Tests.Training
{
    /// <summary>
    /// Helper condition for testing that allows explicitly marking a condition as completed
    /// </summary>
    public class EndlessCondition : Condition<EndlessCondition.EntityData>
    {
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

        public EndlessCondition()
        {
            Data = new EntityData();
        }
    }
}
#endif
