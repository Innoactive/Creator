using System;
using System.Runtime.Serialization;
using VPG.Creator.Core.Exceptions;
using VPG.Creator.Core.Runtime.Properties;

namespace VPG.Creator.Core.SceneObjects
{
    /// <summary>
    /// Base class for references to training scene objects and their properties.
    /// </summary>
    [DataContract(IsReference = true)]
    public abstract class ObjectReference<T> : UniqueNameReference, ICanBeEmpty where T : class
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

        internal override Type GetReferenceType()
        {
            return typeof(T);
        }

        public static implicit operator T(ObjectReference<T> reference)
        {
            return reference.Value;
        }

        protected ObjectReference()
        {
        }

        protected ObjectReference(string uniqueName) : base(uniqueName)
        {
        }

        protected abstract T DetermineValue(T cachedValue);

        /// <inheritdoc/>
        public bool IsEmpty()
        {
            try
            {
                return string.IsNullOrEmpty(UniqueName) || Value == null;
            }
            catch (MissingEntityException)
            {
                return true;
            }
        }
    }
}
