using System;
using System.Linq;
using Innoactive.Creator.Core.Utils;
using UnityEngine;

namespace Innoactive.Creator.Unity
{
    /// <summary>
    /// An UnitySceneSingleton is intended to be destroyed on scene change.
    /// </summary>
    public abstract class UnitySceneSingleton<T> : MonoBehaviour where T : UnitySceneSingleton<T>
    {
        /// <summary>
        /// Semaphore to avoid instantiating the singleton twice.
        /// </summary>
        private static object semaphore = new object();

        public static Type ConcreteType
        {
            get
            {
                if (typeof(T).IsAbstract)
                {
                    return ReflectionUtils.GetConcreteImplementationsOf(typeof(T)).First();
                }

                return typeof(T);
            }
        }

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
                        instance = (T)FindObjectOfType(ConcreteType);
                    }

                    if (instance == null)
                    {
                        GameObject g = new GameObject();
                        instance = (T)g.AddComponent(ConcreteType);
                        g.name = instance.GetName();
                    }
                }

                return instance;
            }

            protected set
            {
                instance = value;
            }
        }

        public static bool Exists
        {
            get { return instance != null; }
        }

        protected virtual string GetName()
        {
            return string.Format("[{0}_SceneSingleton]", typeof(T).Name);
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
                Debug.LogWarningFormat("An instance of the UnitySceneSingleton {0} already exists.", typeof(T).Name);
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
