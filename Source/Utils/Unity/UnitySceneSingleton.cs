﻿using Innoactive.Hub.Logging;
using UnityEngine;

namespace Innoactive.Hub.Training.Unity.Utils
{
    /// <summary>
    /// An UnitySceneSingleton is intended to be destroyed on scene change.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UnitySceneSingleton<T> : MonoBehaviour where T : UnitySceneSingleton<T>
    {
        private static readonly Common.Logging.ILog logger = LogManager.GetLogger("UnitySceneSingleton");

        /// <summary>
        /// Semaphore to avoid instantiating the singleton twice.
        /// </summary>
        private static object semaphore = new object();

        /// <summary>
        /// The actual instance of the singleton object.
        /// </summary>
        private static T instance;

        /// <summary>
        /// Public accessor for the singleton object, will create a new instance if necessary.
        /// </summary>
        public static T Instance
        {
            get
            {
                lock (semaphore)
                {
                    if (instance == null)
                    {
                        instance = FindObjectOfType<T>();
                        if (instance == null)
                        {
                            GameObject g = new GameObject();
                            g.name = string.Format("[{0}_SceneSingleton]", typeof(T).Name);
                            instance = g.AddComponent<T>();
                        }
                    }
                }

                return instance;
            }
            protected set
            {
                instance = value;
            }
        }

        protected virtual void Awake()
        {
            // Make sure to assign the instance on awake.
            if (instance == null)
            {
                instance = (T) this;
            }
            else
            {
                if (Instance == this)
                {
                    return;
                }
                Destroy(gameObject);
                logger.Warn("An instance of the UnitySceneSingleton " + typeof(T).Name + " already exists.");
            }
        }

        protected virtual void OnDestroy()
        {
            // As soon as this singleton is destroyed, clear the instance of it.
            if (instance == this)
            {
                instance = null;
            }
        }
    }
}
