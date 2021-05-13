using System;
using System.Runtime.Serialization;
using VPG.Creator.Core.Configuration;
using VPG.Creator.Core.Properties;

namespace VPG.Creator.Core.SceneObjects
{
    /// <summary>
    /// Weak reference to a property of a training scene object with the same unique name.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class ScenePropertyReference<T> : ObjectReference<T> where T : class, ISceneObjectProperty
    {
        public static implicit operator T(ScenePropertyReference<T> reference)
        {
            return reference.Value;
        }

        public ScenePropertyReference()
        {
        }

        public ScenePropertyReference(string uniqueName) : base(uniqueName)
        {
        }

        protected override T DetermineValue(T cachedValue)
        {
            if (string.IsNullOrEmpty(UniqueName))
            {
                return null;
            }

            T value = cachedValue;

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

            ISceneObject sceneObject = RuntimeConfigurator.Configuration.SceneObjectRegistry.GetByName(UniqueName);

            // Can't find training scene object with given UniqueName, value is null.
            if (sceneObject == null)
            {
                return value;
            }

            value = sceneObject.GetProperty<T>();
            return value;
        }
    }
}
