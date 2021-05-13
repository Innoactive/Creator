﻿using System;
using System.Runtime.Serialization;
using VPG.Creator.Core.Properties;
using VPG.Creator.Core.SceneObjects;

namespace VPG.Creator.Core.Behaviors
{
    /// <summary>
    /// Serializable reference to a <see cref="LockableProperty"/>
    /// </summary>
    [DataContract(IsReference = true)]
    public class LockablePropertyReference
    {
        /// <summary>
        /// Reference to the scene object the LockableProperty is attached to.
        /// </summary>
        [DataMember]
        public SceneObjectReference Target;

        /// <summary>
        /// Type name of the LockableProperty.
        /// </summary>
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

        /// <summary>
        /// Returns the referenced <see cref="LockableProperty"/>.
        /// </summary>
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
