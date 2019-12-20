using System.Runtime.Serialization;
using Innoactive.Hub.Training.Configuration;
using Newtonsoft.Json;

namespace Innoactive.Hub.Training.SceneObjects
{
    /// <summary>
    /// Weak reference by a unique name to a training scene object in a scene.
    /// </summary>
    [DataContract(IsReference = true)]
    public sealed class SceneObjectReference : ObjectReference<ISceneObject>
    {
        [JsonConstructor]
        public SceneObjectReference()
        {
        }

        public SceneObjectReference(string uniqueName) : base(uniqueName)
        {
        }

        protected override ISceneObject DetermineValue(ISceneObject cached)
        {
            if (string.IsNullOrEmpty(UniqueName))
            {
                return null;
            }

            ISceneObject value = cached;

            // If MonoBehaviour was destroyed, nullify the value.
            if (value != null && value.Equals(null))
            {
                value = null;
            }

            // If value exists, return it.
            if (value != null)
            {
                return value;
            }

            value = RuntimeConfigurator.Configuration.SceneObjectRegistry.GetByName(UniqueName);
            return value;
        }
    }
}
