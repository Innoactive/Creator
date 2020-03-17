using System;
using UnityEngine;

namespace Innoactive.Creator.Unity
{
    /// <summary>
    /// Exception raised if no valid game object has been given
    /// </summary>
    public class InvalidGameObjectException : Exception
    {
        public InvalidGameObjectException(string message, Exception innerException = null) : base(message, innerException) { }
    }

    /// <summary>
    /// Helper class facilitating dealing with the management of components to make your life easier without
    /// having to rewrite the same code over and over again
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Whether or not the Component of Type T is already attched to the current game object.
        /// This is the generic version of <see cref="HasComponent"/>
        /// </summary>
        /// <typeparam name="T">the type of component to check for</typeparam>
        /// <param name="gameObject">the game object on which to look for the compnent in question</param>
        /// <returns>whether or not the Component of Type T is already attched to the current game object</returns>
        public static bool HasComponent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.HasComponent(typeof(T));
        }

        /// <summary>
        /// Whether or not the Component of Type T is already attached to the current game object
        /// </summary>
        /// <param name="gameObject">the game object on which to look for the component in question</param>
        /// <param name="componentType">the type of component to check for</param>
        /// <returns>whether or not the Component of Type T is already attached to the current game object</returns>
        public static bool HasComponent(this GameObject gameObject, Type componentType)
        {
            return gameObject.GetComponent(componentType) != null;
        }

        /// <summary>
        /// Returns an instance of the component of type <typeparamref name="T"/>.
        /// If no instance of the component exists on the <paramref name="gameObject"/> yet, a new instance will be created if <paramref name="addIfNonExistent"/> is set to true.
        /// Otherwise, the behaviour is identical to that of `GameObject.GetComponent&lt;T&gt;`.
        /// This is the generic version of <see cref="GetComponent"/>
        /// </summary>
        /// <typeparam name="T">the type of the component of which an instance should be returned</typeparam>
        /// <param name="gameObject">the game object on which to look for a component of the specified type <typeparamref name="T"/></param>
        /// <param name="addIfNonExistent">if set to true, will add the specified component type to the game object if it doesn't exist yet, else it's the standard behaviour of `GameObject.GetComponent&lt;T&gt;`</param>
        /// <returns>the instance of the newly created (or existing) component</returns>
        public static T GetComponent<T>(this GameObject gameObject, bool addIfNonExistent = false) where T : Component
        {
            return (T)GetComponent(gameObject, typeof(T), addIfNonExistent);
        }

        /// <summary>
        /// Returns an instance of the component of <paramref name="componentType"/>.
        /// If no instance of the component exists on the <paramref name="gameObject"/> yet, a new instance will be created if <paramref name="addIfNonExistent"/> is set to true.
        /// Otherwise, the behaviour is identical to that of `GameObject.GetComponent(Type)`.
        /// </summary>
        /// <param name="gameObject">the game object on which to look for a component of the specified type <paramref name="componentType"/></param>
        /// <param name="componentType">the type of the component of which an instance should be returned</param>
        /// <param name="addIfNonExistent">if set to true, will add the specified component type to the game object if it doesn't exist yet, else it's the standard behaviour of `GameObject.GetComponent(Type)`</param>
        /// <returns>the instance of the newly created (or existing) component</returns>
        public static object GetComponent(this GameObject gameObject, Type componentType, bool addIfNonExistent = false)
        {
            if (gameObject == null)
            {
                throw new InvalidGameObjectException(
                    "Please pass a valid gameobject to the AddComponentToGameObjectIfNotExisting method");
            }
            Component component = gameObject.GetComponent(componentType);
            if (component != null)
            {
                return component;
            }
            return gameObject.AddComponent(componentType);
        }

        /// <summary>
        /// Adds a component of class <typeparamref name="T"/> to the game object.
        /// If the parameter <paramref name="onlyAddIfNonExistent"/> is set to true, the component will not be added
        /// if another component of the same class <typeparamref name="T"/> is already attached to the game object.
        /// This is the generic version of <see cref="AddComponent(GameObject,Type,bool)"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject"></param>
        /// <param name="onlyAddIfNonExistent"></param>
        /// <returns></returns>
        public static T AddComponent<T>(this GameObject gameObject, bool onlyAddIfNonExistent = false)
        {
            return (T)gameObject.AddComponent(typeof(T), onlyAddIfNonExistent);
        }

        /// <summary>
        /// Adds a component of class <paramref name="componentType"/> to the game object.
        /// If the parameter <paramref name="onlyAddIfNonExistent"/> is set to true, the component will not be added
        /// if another component of the same class <paramref name="componentType"/> is already attached to the game object.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="componentType"></param>
        /// <param name="onlyAddIfNonExistent"></param>
        /// <returns></returns>
        public static object AddComponent(this GameObject gameObject, Type componentType,
            bool onlyAddIfNonExistent = false)
        {
            if (gameObject == null)
            {
                throw new InvalidGameObjectException(
                    "Please pass a valid gameobject to the AddComponentToGameObjectIfNotExisting method");
            }

            if (onlyAddIfNonExistent)
            {
                return gameObject.GetComponent(componentType, true);
            }
            return gameObject.AddComponent(componentType);
        }
    }
}
