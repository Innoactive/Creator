using System.Collections;
using System.Runtime.Serialization;
using Innoactive.Hub.Training.Attributes;
using Newtonsoft.Json;
using UnityEngine;

namespace Innoactive.Hub.Training.Behaviors
{
    /// <summary>
    /// Behavior that waits for `DelayTime` seconds before finishing its activation.
    /// </summary>
    [DataContract(IsReference = true)]
    public class DelayBehavior : Behavior<DelayBehavior.EntityData>
    {
        [DisplayName("Delay")]
        [DataContract(IsReference = true)]
        public class EntityData : IBehaviorData
        {
            [DataMember]
            [DisplayName("Delay in seconds")]
            public float DelayTime { get; set; }

            public Metadata Metadata { get; set; }

            public string Name { get; set; }
        }

        [JsonConstructor]
        public DelayBehavior() : this(0)
        {
        }

        public DelayBehavior(float delayTime, string name = "Delay")
        {
            if (delayTime < 0f)
            {
                Debug.LogWarningFormat("DelayTime has to be zero or positive, but it was {0}. Setting to 0 instead.", delayTime);
                delayTime = 0f;
            }

            Data = new EntityData
            {
                DelayTime = delayTime,
                Name = name,
            };
        }

        private class ActivatingProcess : IStageProcess<EntityData>
        {
            public void Start(EntityData data)
            {
            }

            public IEnumerator Update(EntityData data)
            {
                float timeStarted = Time.time;

                while (Time.time - timeStarted < data.DelayTime)
                {
                    yield return null;
                }
            }

            public void End(EntityData data)
            {
            }

            public void FastForward(EntityData data)
            {
            }
        }

        private readonly IProcess<EntityData> process = new Process<EntityData>(new ActivatingProcess(), new EmptyStageProcess<EntityData>(), new EmptyStageProcess<EntityData>());

        protected override IProcess<EntityData> Process
        {
            get
            {
                return process;
            }
        }
    }
}
