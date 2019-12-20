#if UNITY_EDITOR
using System.Collections;
using Innoactive.Hub.Training;
using Innoactive.Hub.Training.Behaviors;
using UnityEngine;

namespace Innoactive.Hub.Unity.Tests.Training
{
    /// <summary>
    /// Helper Behavior for testing that provides a behavior with fixed durations for activate and deactivate
    /// </summary>
    public class TimeoutBehavior : Behavior<TimeoutBehavior.EntityData>
    {
        public class EntityData : IBehaviorData
        {
            public float ActivatingTime { get; set; }
            public float DeactivatingTime { get; set; }

            public Metadata Metadata { get; set; }
            public string Name { get; set; }
        }

        public TimeoutBehavior(float activatingTime, float deactivatingTime)
        {
            Data = new EntityData
            {
                ActivatingTime = activatingTime,
                DeactivatingTime = deactivatingTime
            };
        }

        private class ActivatingProcess : IStageProcess<EntityData>
        {
            public void Start(EntityData data)
            {
            }

            public IEnumerator Update(EntityData data)
            {
                float startedAt = Time.time;

                while (Time.time - startedAt < data.ActivatingTime)
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

        private class DeactivatingProcess : IStageProcess<EntityData>
        {
            public void Start(EntityData data)
            {
            }

            public IEnumerator Update(EntityData data)
            {
                float startedAt = Time.time;

                while (Time.time - startedAt < data.DeactivatingTime)
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

        private readonly IProcess<EntityData> process = new Process<EntityData>(new ActivatingProcess(), new EmptyStageProcess<EntityData>(), new DeactivatingProcess());

        protected override IProcess<EntityData> Process
        {
            get
            {
                return process;
            }
        }
    }
}
#endif
