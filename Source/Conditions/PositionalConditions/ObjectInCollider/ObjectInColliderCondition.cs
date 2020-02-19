using System.Runtime.Serialization;
using Innoactive.Hub.Training.Attributes;
using Innoactive.Hub.Training.SceneObjects;
using Innoactive.Hub.Training.SceneObjects.Properties;
using Innoactive.Hub.Training.Utils;

namespace Innoactive.Hub.Training.Conditions
{
    /// <summary>
    /// Condition which is completed when `TargetObject` gets inside `TriggerProperty`'s collider.
    /// </summary>
    [DataContract(IsReference = true)]
    public class ObjectInColliderCondition : Condition<ObjectInColliderCondition.EntityData>
    {
        [DisplayName("Move Object into Collider")]
        [DataContract(IsReference = true)]
        public class EntityData : IObjectInTargetData
        {
            [DataMember]
            [DisplayName("Object to collide into")]
            public ScenePropertyReference<ColliderWithTriggerProperty> TriggerProperty { get; set; }

            [DataMember]
            [DisplayName("Target object")]
            public SceneObjectReference TargetObject { get; set; }

            public bool IsCompleted { get; set; }

            [DataMember]
            [HideInTrainingInspector]
            public string Name { get; set; }

            [DataMember]
            [DisplayName("Required seconds inside")]
            public float RequiredTimeInside { get; set; }

            public Metadata Metadata { get; set; }
        }

        public ObjectInColliderCondition() : this("", "")
        {
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        public ObjectInColliderCondition(ColliderWithTriggerProperty targetPosition, ISceneObject targetObject, float requiredTimeInTarget = 0, string name = null)
            : this(TrainingReferenceUtils.GetNameFrom(targetPosition), TrainingReferenceUtils.GetNameFrom(targetObject), requiredTimeInTarget, name)
        {
        }

        public ObjectInColliderCondition(string targetPosition, string targetObject, float requiredTimeInTarget = 0, string name = "Move Object into Collider")
        {
            Data = new EntityData
            {
                TriggerProperty = new ScenePropertyReference<ColliderWithTriggerProperty>(targetPosition),
                TargetObject = new SceneObjectReference(targetObject),
                RequiredTimeInside = requiredTimeInTarget,
                Name = name
            };
        }

        private class ActiveProcess : ObjectInTargetActiveProcess<EntityData>
        {
            protected override bool IsInside(EntityData data)
            {
                return data.TriggerProperty.Value.IsTransformInsideTrigger(data.TargetObject.Value.GameObject.transform);
            }
        }

        private class EntityAutocompleter : BaseAutocompleter<EntityData>
        {
            public override void Complete(EntityData data)
            {
                data.TriggerProperty.Value.FastForwardEnter(data.TargetObject.Value);
                base.Complete(data);
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

        private readonly IAutocompleter<EntityData> autocompleter = new EntityAutocompleter();
        protected override IAutocompleter<EntityData> Autocompleter
        {
            get
            {
                return autocompleter;
            }
        }
    }
}
