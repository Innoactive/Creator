using System.Collections;
using System.Runtime.Serialization;
using Innoactive.Hub.Training.Attributes;
using Innoactive.Hub.Training.SceneObjects;
using Innoactive.Hub.Training.SceneObjects.Properties;
using Innoactive.Hub.Training.Utils;
using Newtonsoft.Json;

namespace Innoactive.Hub.Training.Conditions
{
    /// <summary>
    /// Condition which is completed when `GrabbableProperty` is grabbed.
    /// </summary>
    [DataContract(IsReference = true)]
    public class GrabbedCondition : Condition<GrabbedCondition.EntityData>
    {
        [DisplayName("Grab Object")]
        public class EntityData : IConditionData
        {
            [DataMember]
            [DisplayNameAttribute("Grabbable object")]
            public ScenePropertyReference<GrabbableProperty> GrabbableProperty { get; set; }

            public bool IsCompleted { get; set; }

            [DataMember]
            [HideInTrainingInspector]
            public string Name { get; set; }

            public Metadata Metadata { get; set; }
        }

        private class EntityAutocompleter : BaseAutocompleter<EntityData>
        {
            public override void Complete(EntityData data)
            {
                data.GrabbableProperty.Value.FastForwardGrab();
                base.Complete(data);
            }
        }

        private class ActiveProcess : IStageProcess<EntityData>
        {
            public void Start(EntityData data)
            {
                data.IsCompleted = false;
            }

            public IEnumerator Update(EntityData data)
            {
                while (data.GrabbableProperty.Value.IsGrabbed == false)
                {
                    yield return null;
                }

                data.IsCompleted = true;
            }

            public void End(EntityData data)
            {
            }

            public void FastForward(EntityData data)
            {
            }
        }

        [JsonConstructor]
        public GrabbedCondition() : this("")
        {
        }

        public GrabbedCondition(GrabbableProperty target, string name = null) : this(TrainingReferenceUtils.GetNameFrom(target), name)
        {
        }

        public GrabbedCondition(string target, string name = "Grab Object")
        {
            Data = new EntityData()
            {
                GrabbableProperty = new ScenePropertyReference<GrabbableProperty>(target),
                Name = name
            };
        }

        private readonly IProcess<EntityData> process = new ActiveOnlyProcess<EntityData>(new ActiveProcess());
        private readonly IAutocompleter<EntityData> autocompleter = new EntityAutocompleter();

        protected override IProcess<EntityData> Process
        {
            get { return process; }
        }

        protected override IAutocompleter<EntityData> Autocompleter
        {
            get { return autocompleter; }
        }
    }
}
