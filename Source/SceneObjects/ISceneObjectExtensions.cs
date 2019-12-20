using System;
using System.Linq;
using System.Collections.Generic;
using Innoactive.Hub.Training.Configuration;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Innoactive.Hub.Training.SceneObjects.Properties
{
    /// <summary>
    /// Helper class that adds functionality to any <see cref="ISceneObject"/>.
    /// </summary>
    public static class ISceneObjectExtensions
    {
        /// <summary>
        /// Ensures that this <see cref="ISceneObject"/>'s UniqueName is not duplicated.
        /// </summary>
        /// <param name="sceneObject"><see cref="ISceneObject"/> to whom the `UniqueName` will be validated.</param>
        /// <param name="baseName">Optional base for this <paramref name="sceneObject"/>'s `UniqueName`.</param>
        public static void SetSuitableName(this ISceneObject sceneObject, string baseName = "")
        {
            int counter = 1;
            string newName = baseName = string.IsNullOrEmpty(baseName) ? sceneObject.GameObject.name : baseName;

            RuntimeConfigurator.Configuration.SceneObjectRegistry.Unregister(sceneObject);
            while (RuntimeConfigurator.Configuration.SceneObjectRegistry.ContainsName(newName))
            {
                newName = string.Format("{0}_{1}", baseName, counter);
                counter++;
            }

            sceneObject.ChangeUniqueName(newName);
        }

        /// <summary>
        /// Adds a <see cref="ISceneObjectProperty"/> of type <typeparamref name="T"/> into this <see cref="ISceneObject"/>.
        /// </summary>
        /// <param name="sceneObject"><see cref="ISceneObject"/> to whom the type <typeparamref name="T"/> will be added.</param>
        /// <typeparam name="T">The type of <see cref="ISceneObjectProperty"/> to be added to <paramref name="sceneObject"/>.</typeparam>
        /// <returns>A reference to the <see cref="ISceneObjectProperty"/> added to <paramref name="sceneObject"/>.</returns>
        public static ISceneObjectProperty AddTrainingProperty<T>(this ISceneObject sceneObject)
        {
            return AddTrainingProperty(sceneObject, typeof(T));
        }

        /// <summary>
        /// Adds a type of <paramref name="trainingProperty"/> into this <see cref="ISceneObject"/>.
        /// </summary>
        /// <param name="sceneObject"><see cref="ISceneObject"/> to whom the <paramref name="trainingProperty"/> will be added.</param>
        /// <param name="trainingProperty"><see cref="ISceneObjectProperty"/> to be added to <paramref name="sceneObject"/>.</param>
        /// <returns>A reference to the <see cref="ISceneObjectProperty"/> added to <paramref name="sceneObject"/>.</returns>
        public static ISceneObjectProperty AddTrainingProperty(this ISceneObject sceneObject, TrainingSceneObjectProperty trainingProperty)
        {
            Type trainingPropertyType = trainingProperty.GetType();
            return AddTrainingProperty(sceneObject, trainingPropertyType);
        }

        /// <summary>
        /// Adds a type of <paramref name="trainingProperty"/> into this <see cref="ISceneObject"/>.
        /// </summary>
        /// <param name="sceneObject"><see cref="ISceneObject"/> to whom the <paramref name="trainingProperty"/> will be added.</param>
        /// <param name="trainingProperty"><see cref="ISceneObjectProperty"/> to be added to <paramref name="sceneObject"/>.</param>
        /// <returns>A reference to the <see cref="ISceneObjectProperty"/> added to <paramref name="sceneObject"/>.</returns>
        public static ISceneObjectProperty AddTrainingProperty(this ISceneObject sceneObject, Component trainingProperty)
        {
            Type trainingPropertyType = trainingProperty.GetType();
            return AddTrainingProperty(sceneObject, trainingPropertyType);
        }

        /// <summary>
        /// Adds a type of <paramref name="trainingProperty"/> into this <see cref="ISceneObject"/>.
        /// </summary>
        /// <param name="sceneObject"><see cref="ISceneObject"/> to whom the <paramref name="trainingProperty"/> will be added.</param>
        /// <param name="trainingProperty">Typo of <see cref="ISceneObjectProperty"/> to be added to <paramref name="sceneObject"/>.</param>
        /// <returns>A reference to the <see cref="ISceneObjectProperty"/> added to <paramref name="sceneObject"/>.</returns>
        public static ISceneObjectProperty AddTrainingProperty(this ISceneObject sceneObject, Type trainingProperty)
        {
            if (AreParametersNullOrInvalid(sceneObject, trainingProperty))
            {
                return null;
            }

            ISceneObjectProperty sceneObjectProperty = sceneObject.GameObject.GetComponent(trainingProperty) as ISceneObjectProperty ?? sceneObject.GameObject.AddComponent(trainingProperty) as ISceneObjectProperty;
            return sceneObjectProperty;
        }

        /// <summary>
        /// Removes <see cref="ISceneObjectProperty"/> of type <typeparamref name="T"/> from this <see cref="ISceneObject"/>.
        /// </summary>
        /// <param name="sceneObject"><see cref="ISceneObject"/> from whom the type <typeparamref name="T"/> will be removed.</param>
        /// <param name="removeDependencies">If true, this method also removes other components that are marked as `RequiredComponent` by <paramref name="trainingProperty"/>.</param>
        /// <param name="excludedFromBeingRemoved">The components in this list will not be removed if any is a dependency of <paramref name="trainingProperty"/>. Only relevant if <paramref name="removeDependencies"/> is true.</param>
        /// <typeparam name="T">The type of <see cref="ISceneObjectProperty"/> to be removed from <paramref name="sceneObject"/>.</typeparam>
        public static void RemoveTrainingProperty<T>(this ISceneObject sceneObject, bool removeDependencies = false, Component[] excludedFromBeingRemoved = null)
        {
            RemoveTrainingProperty(sceneObject, typeof(T), removeDependencies, excludedFromBeingRemoved);
        }

        /// <summary>
        /// Removes type of <paramref name="trainingProperty"/> from this <see cref="ISceneObject"/>.
        /// </summary>
        /// <param name="sceneObject"><see cref="ISceneObject"/> from whom the <paramref name="trainingProperty"/> will be removed.</param>
        /// <param name="trainingProperty"><see cref="ISceneObjectProperty"/> to be removed from <paramref name="sceneObject"/>.</param>
        /// <param name="removeDependencies">If true, this method also removes other components that are marked as `RequiredComponent` by <paramref name="trainingProperty"/>.</param>
        /// <param name="excludedFromBeingRemoved">The components in this list will not be removed if any is a dependency of <paramref name="trainingProperty"/>. Only relevant if <paramref name="removeDependencies"/> is true.</param>
        public static void RemoveTrainingProperty(this ISceneObject sceneObject, TrainingSceneObjectProperty trainingProperty, bool removeDependencies = false, Component[] excludedFromBeingRemoved = null)
        {
            Type trainingPropertyType = trainingProperty.GetType();
            RemoveTrainingProperty(sceneObject, trainingPropertyType, removeDependencies, excludedFromBeingRemoved);
        }

        /// <summary>
        /// Removes type of <paramref name="trainingProperty"/> from this <see cref="ISceneObject"/>.
        /// </summary>
        /// <param name="sceneObject"><see cref="ISceneObject"/> from whom the <paramref name="trainingProperty"/> will be removed.</param>
        /// <param name="trainingProperty"><see cref="ISceneObjectProperty"/> to be removed from <paramref name="sceneObject"/>.</param>
        /// <param name="removeDependencies">If true, this method also removes other components that are marked as `RequiredComponent` by <paramref name="trainingProperty"/>.</param>
        /// <param name="excludedFromBeingRemoved">The components in this list will not be removed if any is a dependency of <paramref name="trainingProperty"/>. Only relevant if <paramref name="removeDependencies"/> is true.</param>
        public static void RemoveTrainingProperty(this ISceneObject sceneObject, Component trainingProperty, bool removeDependencies = false, Component[] excludedFromBeingRemoved = null)
        {
            Type trainingPropertyType = trainingProperty.GetType();
            RemoveTrainingProperty(sceneObject, trainingPropertyType, removeDependencies, excludedFromBeingRemoved);
        }

        /// <summary>
        /// Removes type of <paramref name="trainingProperty"/> from this <see cref="ISceneObject"/>.
        /// </summary>
        /// <param name="sceneObject"><see cref="ISceneObject"/> from whom the <paramref name="trainingProperty"/> will be removed.</param>
        /// <param name="trainingProperty">Typo of <see cref="ISceneObjectProperty"/> to be removed from <paramref name="sceneObject"/>.</param>
        /// <param name="removeDependencies">If true, this method also removes other components that are marked as `RequiredComponent` by <paramref name="trainingProperty"/>.</param>
        /// <param name="excludedFromBeingRemoved">The components in this list will not be removed if any is a dependency of <paramref name="trainingProperty"/>. Only relevant if <paramref name="removeDependencies"/> is true.</param>
        public static void RemoveTrainingProperty(this ISceneObject sceneObject, Type trainingProperty, bool removeDependencies = false, Component[] excludedFromBeingRemoved = null)
        {
            Component trainingComponent = sceneObject.GameObject.GetComponent(trainingProperty);

            if (AreParametersNullOrInvalid(sceneObject, trainingProperty) || trainingComponent == null)
            {
                return;
            }

            IEnumerable<Component> trainingProperties = sceneObject.GameObject.GetComponents(typeof(Component)).Where(component => component.GetType() != trainingProperty);

            foreach (Component component in trainingProperties)
            {
                if (IsTypeDependencyOfComponent(trainingProperty, component))
                {
                    RemoveTrainingProperty(sceneObject, component);
                }
            }

#if UNITY_EDITOR
            Object.DestroyImmediate(trainingComponent);
#else
            Object.Destroy(trainingComponent);
#endif

            if (removeDependencies)
            {
                List<Type> dependencies = GetAllDependenciesFrom(trainingProperty);

                if (dependencies == null)
                {
                    return;
                }

                foreach (Type dependency in dependencies)
                {
                    if (excludedFromBeingRemoved != null && excludedFromBeingRemoved.Any(d => d.GetType() == dependency))
                    {
                        continue;
                    }

                    RemoveTrainingProperty(sceneObject, dependency, removeDependencies);
                }
            }
        }

        private static bool AreParametersNullOrInvalid(ISceneObject sceneObject, Type trainingProperty)
        {
            return sceneObject == null || sceneObject.GameObject == null || trainingProperty == null || typeof(ISceneObjectProperty).IsAssignableFrom(trainingProperty) == false;
        }

        private static bool IsTypeDependencyOfComponent(Type type, Component component)
        {
            Type propertyType = component.GetType();
            RequireComponent[] requireComponents = propertyType.GetCustomAttributes(typeof(RequireComponent), false) as RequireComponent[];

            if (requireComponents == null || requireComponents.Length == 0)
            {
                return false;
            }

            return requireComponents.Any(requireComponent => requireComponent.m_Type0 == type || requireComponent.m_Type1 == type || requireComponent.m_Type2 == type);
        }

        private static List<Type> GetAllDependenciesFrom(Type trainingProperty)
        {
            RequireComponent[] requireComponents = trainingProperty.GetCustomAttributes(typeof(RequireComponent), false) as RequireComponent[];

            if (requireComponents == null || requireComponents.Length == 0)
            {
                return null;
            }

            List<Type> listOfDependencies = new List<Type>();

            foreach (RequireComponent requireComponent in requireComponents)
            {
                if (requireComponent.m_Type0 != null)
                {
                    listOfDependencies.Add(requireComponent.m_Type0);
                }

                if (requireComponent.m_Type1 != null)
                {
                    listOfDependencies.Add(requireComponent.m_Type1);
                }

                if (requireComponent.m_Type2 != null)
                {
                    listOfDependencies.Add(requireComponent.m_Type2);
                }
            }

            return listOfDependencies;
        }
    }
}
