using System;
using System.Collections.Generic;
using System.Linq;
using Innoactive.Hub.Training.Exceptions;
using Innoactive.Hub.Training.Unity.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Innoactive.Hub.Training.SceneObjects
{
    /// <inheritdoc />
    public class SceneObjectRegistry : ISceneObjectRegistry
    {
        private readonly Dictionary<Guid, ISceneObject> registeredEntities = new Dictionary<Guid, ISceneObject>();

        /// <inheritdoc />
        public ISceneObject this[Guid guid]
        {
            get
            {
                return GetByGuid(guid);
            }
        }

        public SceneObjectRegistry()
        {
            RegisterAll();
        }

        /// <inheritdoc />
        public void RegisterAll()
        {
            foreach (TrainingSceneObject trainingObject in SceneUtils.GetActiveAndInactiveComponents<TrainingSceneObject>())
            {
                try
                {
                    Register(trainingObject);
                }
                catch (NameNotUniqueException)
                {
                    Debug.LogErrorFormat("Registration of training scene object entity with name '{0}' failed. Name is not unique! Entity will destroy itself. Referenced game object: '{1}'.", trainingObject.UniqueName, trainingObject.GameObject.name);
                    Object.DestroyImmediate(trainingObject.gameObject);
                }
                catch (AlreadyRegisteredException)
                {
                }
            }
        }

        /// <inheritdoc />
        public ISceneObject this[string name]
        {
            get
            {
                return GetByName(name);
            }
        }

        /// <inheritdoc />
        public void Register(ISceneObject obj)
        {
            if (ContainsGuid(obj.Guid))
            {
                throw new AlreadyRegisteredException(obj);
            }

            if (ContainsName(obj.UniqueName))
            {
                throw new NameNotUniqueException(obj);
            }

            registeredEntities.Add(obj.Guid, obj);
        }

        /// <inheritdoc />
        public bool Unregister(ISceneObject entity)
        {
            return registeredEntities.Remove(entity.Guid);
        }

        /// <inheritdoc />
        public bool ContainsName(string name)
        {
            return registeredEntities.Any(entity => entity.Value.UniqueName == name);
        }

        /// <inheritdoc />
        public ISceneObject GetByName(string name)
        {
            if (ContainsName(name) == false)
            {
                throw new MissingEntityException(string.Format("Could not find scene entity '{0}'", name));
            }

            return registeredEntities.First(entity => entity.Value.UniqueName == name).Value;
        }

        /// <inheritdoc />
        public bool ContainsGuid(Guid guid)
        {
            return registeredEntities.ContainsKey(guid);
        }

        /// <inheritdoc />
        public ISceneObject GetByGuid(Guid guid)
        {
            try
            {
                return registeredEntities[guid];
            }
            catch (KeyNotFoundException)
            {
                throw new MissingEntityException(string.Format("Could not find scene entity with identifier '{0}'", guid.ToString()));
            }
        }
    }
}
