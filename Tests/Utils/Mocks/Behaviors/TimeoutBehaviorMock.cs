using System.Collections;
using VPG.Core;
using VPG.Core.Behaviors;
using UnityEngine;

namespace VPG.Tests.Utils.Mocks
{
    /// <summary>
    /// Helper Behavior for testing that provides a behavior with fixed durations for activate and deactivate
    /// </summary>
    public class TimeoutBehaviorMock : Behavior<TimeoutBehaviorMock.EntityData>
    {
        public class EntityData : IBehaviorData
        {
            public float ActivatingTime { get; set; }
            public float DeactivatingTime { get; set; }

            public Metadata Metadata { get; set; }
            public string Name { get; set; }
        }

        public TimeoutBehaviorMock(float activatingTime, float deactivatingTime)
        {
            Data.ActivatingTime = activatingTime;
            Data.DeactivatingTime = deactivatingTime;
        }

        private class ActivatingProcess : Process<EntityData>
        {
            public ActivatingProcess(EntityData data) : base(data)
            {
            }

            public override void Start()
            {
            }

            public override IEnumerator Update()
            {
                float startedAt = Time.time;

                while (Time.time - startedAt < Data.ActivatingTime)
                {
                    yield return null;
                }
            }

            public override void End()
            {
            }

            public override void FastForward()
            {
            }
        }

        private class DeactivatingProcess : Process<EntityData>
        {
            public DeactivatingProcess(EntityData data) : base(data)
            {
            }

            public override void Start()
            {
            }

            public override IEnumerator Update()
            {
                float startedAt = Time.time;

                while (Time.time - startedAt < Data.DeactivatingTime)
                {
                    yield return null;
                }
            }

            public override void End()
            {
            }

            public override void FastForward()
            {
            }
        }

        public override IProcess GetActivatingProcess()
        {
            return new ActivatingProcess(Data);
        }

        public override IProcess GetDeactivatingProcess()
        {
            return new DeactivatingProcess(Data);
        }
    }
}
