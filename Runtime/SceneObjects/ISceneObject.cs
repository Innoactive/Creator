﻿using System;
using System.Collections.Generic;
 using VPG.Creator.Core.Properties;
 using UnityEngine;

namespace VPG.Creator.Core.SceneObjects
{
    public class SceneObjectNameChanged : EventArgs
    {
        public readonly string NewName;
        public readonly string PreviousName;

        public SceneObjectNameChanged(string newName, string previousName)
        {
            NewName = newName;
            PreviousName = previousName;
        }
    }

    /// <summary>
    /// Gives the possibility to easily identify targets for Conditions, Behaviors and so on.
    /// </summary>
    public interface ISceneObject : ILockable
    {
        event EventHandler<SceneObjectNameChanged> UniqueNameChanged;

        /// <summary>
        /// Unique Guid for each entity, which is required
        /// </summary>
        Guid Guid { get; }

        /// <summary>
        /// Unique name which is not required
        /// </summary>
        string UniqueName { get; }

        /// <summary>
        /// Target GameObject, used for applying stuff.
        /// </summary>
        GameObject GameObject { get; }

        ICollection<ISceneObjectProperty> Properties { get; }

        bool CheckHasProperty<T>() where T : ISceneObjectProperty;

        bool CheckHasProperty(Type type);

        void ValidateProperties(IEnumerable<Type> properties);

        T GetProperty<T>() where T : ISceneObjectProperty;

        void ChangeUniqueName(string newName);
    }
}
