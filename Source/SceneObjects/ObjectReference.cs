using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Innoactive.Hub.Training.SceneObjects
{
    /// <summary>
    /// Base class for references to training scene objects and their properties.
    /// </summary>
    [DataContract(IsReference = true)]
    public abstract class ObjectReference<T> : UniqueNameReference where T : class
    {
        public override string UniqueName
        {
            get
            {
                return base.UniqueName;
            }
            set
            {
                if (base.UniqueName != value)
                {
                    cachedValue = null;
                }

                base.UniqueName = value;
            }
        }

        private T cachedValue;

        public T Value
        {
            get
            {
                cachedValue = DetermineValue(cachedValue);
                return cachedValue;
            }
        }

        public static implicit operator T(ObjectReference<T> reference)
        {
            return reference.Value;
        }

        [JsonConstructor]
        protected ObjectReference()
        {
        }

        protected ObjectReference(string uniqueName) : base(uniqueName)
        {
        }

        protected abstract T DetermineValue(T cachedValue);
    }
}
