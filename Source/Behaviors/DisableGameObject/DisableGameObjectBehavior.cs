using System.Runtime.Serialization;
using Innoactive.Hub.Training.Attributes;
using Innoactive.Hub.Training.SceneObjects;
using Innoactive.Hub.Training.Utils;
using Newtonsoft.Json;

namespace Innoactive.Hub.Training.Behaviors
{
    /// <summary>
    /// Disables gameObject of target ISceneObject.
    /// </summary>
    [DataContract(IsReference = true)]
    public class DisableGameObjectBehavior : Behavior<DisableGameObjectBehavior.EntityData>
    {
        [DisplayName("Disable Object")]
        [DataContract(IsReference = true)]
        public class EntityData : IBehaviorData
        {
            [DataMember]
            [DisplayName("Object to disable")]
            public SceneObjectReference Target { get; set; }

            public Metadata Metadata { get; set; }
            public string Name { get; set; }
        }

        private class ActivatingProcess : InstantStageProcess<EntityData>
        {
            public override void Start(EntityData data)
            {
                data.Target.Value.GameObject.SetActive(false);
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

        [JsonConstructor]
        public DisableGameObjectBehavior() : this("")
        {
        }

        /// <param name="targetObject">scene object to disable.</param>
        public DisableGameObjectBehavior(ISceneObject targetObject) : this(TrainingReferenceUtils.GetNameFrom(targetObject))
        {
        }

        /// <param name="targetObject">Unique name of target scene object.</param>
        public DisableGameObjectBehavior(string targetObject, string name = "Disable Object")
        {
            Data = new EntityData();
            Data.Target = new SceneObjectReference(targetObject);
            Data.Name = name;
        }
    }
}
