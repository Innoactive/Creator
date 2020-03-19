using System.Runtime.Serialization;
using Innoactive.Creator.Core.Attributes;
using UnityEngine;

namespace Innoactive.Creator.Core.Conditions
{
    [DataContract(IsReference = true)]
    public class TimeoutCondition : Condition<TimeoutCondition.EntityData>
    {
        [DisplayName("Timeout")]
        public class EntityData : IConditionData
        {
            [DataMember]
            [DisplayName("Wait for seconds")]
            public float Timeout { get; set; }

            public bool IsCompleted { get; set; }

            [DataMember]
            [HideInTrainingInspector]
            public string Name { get; set; }

            public Metadata Metadata { get; set; }
        }

        private class ActiveProcess : BaseStageProcessOverCompletable<EntityData>
        {
            private float timeStarted;

            protected override bool CheckIfCompleted(EntityData data)
            {
                return Time.time - timeStarted >= data.Timeout;
            }

            public override void Start(EntityData data)
            {
                timeStarted = Time.time;
                base.Start(data);
            }
        }

        public TimeoutCondition() : this(0)
        {
        }

        public TimeoutCondition(float timeout, string name = "Timeout")
        {
            Data = new EntityData()
            {
                Timeout = timeout,
                Name = name
            };
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
    }
}
