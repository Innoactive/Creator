using System.Linq;
using System.Collections.Generic;
using Innoactive.Creator.Core.Configuration;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Innoactive.Creator.Unity
{
    public static class SceneUtils
    {
        public const string TrainingConfigurationName = "[TRAINING_CONFIGURATION]";

        /// <summary>
        /// Get all loaded components, both active and inactive.
        /// </summary>
        public static IEnumerable<T> GetActiveAndInactiveComponents<T>() where T : Component
        {
            return Resources.FindObjectsOfTypeAll<T>().Where(component =>
            {
                GameObject gameObject = component.gameObject;

                return gameObject != null
                    && gameObject.Equals(null) == false
                    && gameObject.scene.IsValid()
                    && SceneManager.GetActiveScene() == gameObject.scene;
            });
        }

        /// <summary>
        /// Get all loaded game objects, both active and inactive.
        /// </summary>
        public static IEnumerable<GameObject> GetActiveAndInactiveGameObjects()
        {
            return Resources.FindObjectsOfTypeAll<GameObject>().Where(gameObject => gameObject != null && gameObject.Equals(null) == false && gameObject.scene.IsValid() && SceneManager.GetActiveScene() == gameObject.scene);
        }

        /// <summary>
        /// Returns the component of Type <paramref name="T" /> in the <paramref name="gameObject" /> or any of its parents, null if there isn't one.
        /// In contrast to <see cref="UnityEngine.GameObject.GetComponentInParent"/> this method also includes inactive components.
        /// </summary>
        public static T GetComponentInParentIncludingInactive<T>(this GameObject gameObject) where T : Component
        {
            Transform parent = gameObject.transform.parent;

            while (parent != null)
            {
                T component = parent.GetComponent<T>();
                if (component != null)
                {
                    return component;
                }

                parent = parent.parent;
            }

            return null;
        }

        /// <summary>
        /// Returns an instance of the component of type <typeparamref name="T"/>.
        /// If no instance of the component exists on the <paramref name="gameObject"/> yet, a new instance will be created.
        /// Otherwise, the behaviour is identical to that of `GameObject.GetComponent&lt;T&gt;`.
        /// </summary>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            return component == null ? gameObject.AddComponent<T>() : component;
        }

        /// <summary>
        /// Checks whether a <paramref name="gameObject"/> contains missing scripts.
        /// </summary>
        /// <param name="gameObject">GameObject to check.</param>
        /// <returns>True, if scripts are missing.</returns>
        public static bool ContainsMissingScripts(GameObject gameObject)
        {
            foreach (Component component in gameObject.GetComponents<Component>())
            {
                if (component == null)
                {
                    return true;
                }
            }

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                if (ContainsMissingScripts(gameObject.transform.GetChild(i).gameObject))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
