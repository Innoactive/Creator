using System.Runtime.Serialization;
using Innoactive.Hub.Training.Attributes;
using Innoactive.Hub.Training.SceneObjects;
using Innoactive.Hub.Training.Utils;
using Newtonsoft.Json;

namespace Innoactive.Hub.Training.Behaviors
{
    /// <summary>
    /// Enables gameObject of target ISceneObject.
    /// </summary>
    [DataContract(IsReference = true)]
    public class EnableGameObjectBehavior : Behavior<EnableGameObjectBehavior.EntityData>
    {
        [DisplayName("Enable Object")]
        [DataContract(IsReference = true)]
        public class EntityData : IBehaviorData
        {
            [DataMember]
            [DisplayName("Object to enable")]
            public SceneObjectReference Target { get; set; }

            public Metadata Metadata { get; set; }
            public string Name { get; set; }
        }

        private class ActivatingProcess : InstantStageProcess<EntityData>
        {
            public override void Start(EntityData data)
            {
                data.Target.Value.GameObject.SetActive(true);
            }
        }

        [JsonConstructor]
        public EnableGameObjectBehavior() : this("")
        {
        }

        /// <param name="targetObject">Object to enable.</param>
        public EnableGameObjectBehavior(ISceneObject targetObject) : this(TrainingReferenceUtils.GetNameFrom(targetObject))
        {
        }

        /// <param name="targetObject">Name of the object to enable.</param>
        public EnableGameObjectBehavior(string targetObject, string name = "Enable Object")
        {
            Data = new EntityData();
            Data.Target = new SceneObjectReference(targetObject);
            Data.Name = name;
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
