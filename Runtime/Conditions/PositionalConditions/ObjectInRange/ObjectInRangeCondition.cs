using System.Runtime.Serialization;
using Innoactive.Creator.Core.Attributes;
using Innoactive.Creator.Core.SceneObjects;
using Innoactive.Creator.Core.SceneObjects.Properties;
using Innoactive.Creator.Core.Utils;

namespace Innoactive.Creator.Core.Conditions
{
    /// <summary>
    /// Condition that is completed when distance between `Target` and `TransformInRangeDetector` is closer than `range` units.
    /// </summary>
    [DataContract(IsReference = true)]
    public class ObjectInRangeCondition : Condition<ObjectInRangeCondition.EntityData>
    {
        [DisplayName("Object Nearby")]
        public class EntityData : IObjectInTargetData
        {
            [DataMember]
            [DisplayName("First object")]
            public SceneObjectReference DistanceDetector { get; set; }

            [DataMember]
            [DisplayName("Second object")]
            public SceneObjectReference Target { get; set; }

            [DataMember]
            public float Range { get; set; }

            [DataMember]
            [HideInTrainingInspector]
            public string Name { get; set; }

            [DataMember]
            [DisplayName("Required seconds inside")]
            public float RequiredTimeInside { get; set; }

            public bool IsCompleted { get; set; }

            public Metadata Metadata { get; set; }
        }

        public ObjectInRangeCondition() : this("", "", 0f)
        {
        }

        public ObjectInRangeCondition(ISceneObject target, TransformInRangeDetectorProperty detector, float range, float requiredTimeInTarget = 0, string name = null)
            : this(TrainingReferenceUtils.GetNameFrom(target), TrainingReferenceUtils.GetNameFrom(detector), range, requiredTimeInTarget, name)
        {
        }

        public ObjectInRangeCondition(string target, string detector, float range, float requiredTimeInTarget = 0, string name = "Object Nearby")
        {
            Data = new EntityData()
            {
                Target = new SceneObjectReference(target),
                DistanceDetector = new SceneObjectReference(detector),
                Range = range,
                RequiredTimeInside = requiredTimeInTarget,
                Name = name
            };
        }

        private class ActiveProcess : ObjectInTargetActiveProcess<EntityData>
        {
            protected override bool IsInside(EntityData data)
            {
                return (data.Target.Value.GameObject.transform.position - data.DistanceDetector.Value.GameObject.transform.position).magnitude <= data.Range;
            }
        }

        private class EntityAutocompleter : BaseAutocompleter<EntityData>
        {
            public override void Complete(EntityData data)
            {
                data.Target.Value.GameObject.transform.position = data.DistanceDetector.Value.GameObject.transform.position;
                data.Target.Value.GameObject.transform.rotation = data.DistanceDetector.Value.GameObject.transform.rotation;
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
