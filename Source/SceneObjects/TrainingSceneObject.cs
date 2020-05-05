﻿using UnityEngine;
using System;
using System.Collections.Generic;
using Innoactive.Hub.Training.Exceptions;
using Innoactive.Hub.Training.Configuration;
using Innoactive.Hub.Training.SceneObjects.Properties;

namespace Innoactive.Hub.Training.SceneObjects
{
    /// <inheritdoc cref="ISceneObject"/>
    [ExecuteInEditMode]
    public class TrainingSceneObject : MonoBehaviour, ISceneObject
    {
        public event EventHandler<LockStateChangedEventArgs> Locked;
        public event EventHandler<LockStateChangedEventArgs> Unlocked;
        public event EventHandler<SceneObjectNameChanged> UniqueNameChanged;

        public GameObject GameObject
        {
            get { return gameObject; }
        }

        [SerializeField]
        [Tooltip("Unique name which identifies an object in scene, can be null or empty, but has to be unique in the scene.")]
        private string uniqueName = null;

        /// <inheritdoc />
        public string UniqueName
        {
            get
            {
                if (string.IsNullOrEmpty(uniqueName))
                {
                    return "REF-" + Guid;
                }

                return uniqueName;
            }
        }

        private Guid guid = Guid.NewGuid();

        /// <inheritdoc />
        public Guid Guid
        {
            get { return guid; }
        }

        public ICollection<ISceneObjectProperty> Properties
        {
            get { return GetComponents<ISceneObjectProperty>(); }
        }

        public bool IsLocked { get; private set; }

        private bool IsRegistered
        {
            get { return RuntimeConfigurator.Configuration.SceneObjectRegistry.ContainsGuid(Guid); }
        }

        private void Awake()
        {
            if (IsRegistered)
            {
                return;
            }

            this.SetSuitableName();
        }

        private void OnDestroy()
        {
            if (RuntimeConfigurator.Exists)
            {
                RuntimeConfigurator.Configuration.SceneObjectRegistry.Unregister(this);
            }
        }

        public bool CheckHasProperty<T>() where T : ISceneObjectProperty
        {
            return CheckHasProperty(typeof(T));
        }

        public bool CheckHasProperty(Type type)
        {
            return FindProperty(type) != null;
        }

        public T GetProperty<T>() where T : ISceneObjectProperty
        {
            ISceneObjectProperty property = FindProperty(typeof(T));
            if (property == null)
            {
                throw new PropertyNotFoundException(this, typeof(T));
            }

            return (T)property;
        }

        public void ValidateProperties(IEnumerable<Type> properties)
        {
            bool hasFailed = false;
            foreach (Type propertyType in properties)
            {
                // ReSharper disable once InvertIf
                if (CheckHasProperty(propertyType) == false)
                {
                    Debug.LogErrorFormat("Property of type '{0}' is not attached to SceneObject '{1}'", propertyType.Name, UniqueName);
                    hasFailed = true;
                }
            }

            if (hasFailed)
            {
                throw new PropertyNotFoundException("One or more SceneObjectProperties could not be found, check your log entries for more information.");
            }
        }

        public void SetLocked(bool lockState)
        {
            if (IsLocked == lockState)
            {
                return;
            }

            IsLocked = lockState;

            if (IsLocked)
            {
                if (Locked != null)
                {
                    Locked.Invoke(this, new LockStateChangedEventArgs(IsLocked));
                }
            }
            else
            {
                if (Unlocked != null)
                {
                    Unlocked.Invoke(this, new LockStateChangedEventArgs(IsLocked));
                }
            }
        }

        /// <summary>
        /// Tries to find property which is assignable to given type, this method
        /// will return null if none is found.
        /// </summary>
        private ISceneObjectProperty FindProperty(Type type)
        {
            return GetComponent(type) as ISceneObjectProperty;
        }

        public void ChangeUniqueName(string newName)
        {
            if (newName == UniqueName)
            {
                return;
            }

            if (RuntimeConfigurator.Configuration.SceneObjectRegistry.ContainsName(newName))
            {
                Debug.LogErrorFormat("An object with a name '{0}' is already registered. The new name is ignored. The name is still '{1}'.", newName, UniqueName);
                return;
            }

            string previousName = UniqueName;

            if (IsRegistered)
            {
                RuntimeConfigurator.Configuration.SceneObjectRegistry.Unregister(this);
            }

            uniqueName = newName;

            RuntimeConfigurator.Configuration.SceneObjectRegistry.Register(this);

            if (UniqueNameChanged != null)
            {
                UniqueNameChanged.Invoke(this, new SceneObjectNameChanged(UniqueName, previousName));
            }
        }
    }
}
