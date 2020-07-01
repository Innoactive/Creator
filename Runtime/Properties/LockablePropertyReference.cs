using System;
using System.Runtime.Serialization;
using Innoactive.Creator.Core.Properties;
using Innoactive.Creator.Core.SceneObjects;

namespace Innoactive.Creator.Core.Behaviors
{
    /// <summary>
    /// Serializable reference to a <see cref="LockableProperty"/>
    /// </summary>
    [DataContract(IsReference = true)]
    public class LockablePropertyReference
    {
        [DataMember]
        public SceneObjectReference Target;

        [DataMember]
        public string Type;

        [IgnoreDataMember]
        private LockableProperty property;

        public LockablePropertyReference()
        {

        }

        public LockablePropertyReference(LockableProperty property)
        {
            Target = new SceneObjectReference(property.SceneObject.UniqueName);
            Type = property.GetType().AssemblyQualifiedName;
        }

        public LockablePropertyReference(string sceneObjectName, Type type)
        {
            Target = new SceneObjectReference(sceneObjectName);
            Type = type.AssemblyQualifiedName;
        }

        public LockableProperty GetProperty()
        {
            if (property == null)
            {
                foreach (ISceneObjectProperty prop in Target.Value.Properties)
                {
                    if (prop.GetType().AssemblyQualifiedName.Equals(Type))
                    {
                        property = (LockableProperty)prop;
                        break;
                    }
                }
            }

            return property;
        }
    }
}
