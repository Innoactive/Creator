using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using Innoactive.Creator.Core.Attributes;
using Innoactive.Creator.Core.SceneObjects;
using Innoactive.Creator.Core.Utils;

namespace Innoactive.Creator.Core.Behaviors
{
    /// <summary>
    /// Behavior that moves target SceneObject to the position and rotation of another TargetObject.
    /// It takes `Duration` seconds, even if the target was in the place already.
    /// If `Duration` is equal or less than 0, transition is instantaneous.
    /// </summary>
    [DataContract(IsReference = true)]
    public class MoveObjectBehavior : Behavior<MoveObjectBehavior.EntityData>
    {
        [DisplayName("Move Object")]
        [DataContract(IsReference = true)]
        public class EntityData : IBehaviorData
        {
            /// <summary>
            /// Target scene object to be moved.
            /// </summary>
            [DataMember]
            [DisplayName("Object to move")]
            public SceneObjectReference Target { get; set; }

            /// <summary>
            /// Target's position and rotation is linearly interpolated to match PositionProvider's position and rotation at the end of transition.
            /// </summary>
            [DataMember]
            [DisplayName("Final position provider")]
            public SceneObjectReference PositionProvider { get; set; }

            /// <summary>
            /// Duration of the transition. If duration is equal or less than zero, target object movement is instantaneous.
            /// </summary>
            [DataMember]
            [DisplayName("Duration in seconds")]
            public float Duration { get; set; }

            public Metadata Metadata { get; set; }
            public string Name { get; set; }
        }

        private class ActivatingProcess : IStageProcess<EntityData>
        {
            private float startingTime;
            public void Start(EntityData data)
            {
                startingTime = Time.time;
            }

            public IEnumerator Update(EntityData data)
            {
                Transform movingTransform = data.Target.Value.GameObject.transform;
                Transform targetPositionTransform = data.PositionProvider.Value.GameObject.transform;

                Vector3 initialPosition = movingTransform.position;
                Quaternion initialRotation = movingTransform.rotation;

                while (Time.time - startingTime < data.Duration)
                {
                    if (movingTransform == null || movingTransform.Equals(null) || targetPositionTransform == null || targetPositionTransform.Equals(null))
                    {
                        string warningFormat = "The training scene object's game object is null, transition movement is not completed, behavior activation is forcefully finished.";
                        warningFormat += "Target object unique name: {0}, Position provider's unique name: {1}";
                        Debug.LogWarningFormat(warningFormat, data.Target.UniqueName, data.PositionProvider.UniqueName);
                        yield break;
                    }

                    float progress = (Time.time - startingTime) / data.Duration;

                    movingTransform.position = Vector3.Lerp(initialPosition, targetPositionTransform.position, progress);
                    movingTransform.rotation = Quaternion.Slerp(initialRotation, targetPositionTransform.rotation, progress);

                    yield return null;
                }
            }

            public void End(EntityData data)
            {
                Transform movingTransform = data.Target.Value.GameObject.transform;
                Transform targetPositionTransform = data.PositionProvider.Value.GameObject.transform;

                movingTransform.position = targetPositionTransform.position;
                movingTransform.rotation = targetPositionTransform.rotation;
            }

            public void FastForward(EntityData data)
            {
            }
        }

        public MoveObjectBehavior() : this("", "", 0f)
        {
        }

        public MoveObjectBehavior(ISceneObject target, ISceneObject positionProvider, float duration) : this(TrainingReferenceUtils.GetNameFrom(target), TrainingReferenceUtils.GetNameFrom(positionProvider), duration)
        {
        }

        public MoveObjectBehavior(string targetName, string positionProviderName, float duration, string name = "Move Object")
        {
            Data = new EntityData()
            {
                Target = new SceneObjectReference(targetName),
                PositionProvider = new SceneObjectReference(positionProviderName),
                Duration = duration,
                Name = name
            };
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
