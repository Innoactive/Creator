using System.Collections;
using System.Runtime.Serialization;
using Common.Logging;
using Innoactive.Hub.Training.Attributes;
using UnityEngine;
using LogManager = Innoactive.Hub.Logging.LogManager;

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

        private static readonly ILog logger = LogManager.GetLogger<DelayBehavior>();

        public DelayBehavior() : this(0)
        {
        }

        public DelayBehavior(float delayTime, string name = "Delay")
        {
            if (delayTime < 0f)
            {
                logger.WarnFormat("DelayTime has to be zero or positive, but it was {0}. Setting to 0 instead.", delayTime);
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
