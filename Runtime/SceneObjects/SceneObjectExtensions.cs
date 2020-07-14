using System;
using System.Linq;
using System.Collections.Generic;
using Innoactive.Creator.Core.Configuration;
using Innoactive.Creator.Core.SceneObjects;
using Innoactive.Creator.Core.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Innoactive.Creator.Core.Properties
{
    /// <summary>
    /// Helper class that adds functionality to any <see cref="ISceneObject"/>.
    /// </summary>
    public static class SceneObjectExtensions
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
        /// <param name="trainingProperty">Typo of <see cref="ISceneObjectProperty"/> to be added to <paramref name="sceneObject"/>.</param>
        /// <returns>A reference to the <see cref="ISceneObjectProperty"/> added to <paramref name="sceneObject"/>.</returns>
        public static ISceneObjectProperty AddTrainingProperty(this ISceneObject sceneObject, Type trainingProperty)
        {
            if (AreParametersNullOrInvalid(sceneObject, trainingProperty))
            {
                return null;
            }

            ISceneObjectProperty sceneObjectProperty = sceneObject.GameObject.GetComponent(trainingProperty) as ISceneObjectProperty;

            if (sceneObjectProperty != null)
            {
                return sceneObjectProperty;
            }

            if (trainingProperty.IsInterface)
            {
                // If it is an interface just take the first public found concrete implementation.
                Type propertyType = ReflectionUtils
                    .GetAllTypes()
                    .Where(trainingProperty.IsAssignableFrom)
                    .Where(type => type.Assembly.GetReferencedAssemblies().All(assemblyName =>  assemblyName.Name != "UnityEditor" && assemblyName.Name != "nunit.framework"))
                    .First(type => type.IsClass && type.IsPublic && type.IsAbstract == false);

                sceneObjectProperty = sceneObject.GameObject.AddComponent(propertyType) as ISceneObjectProperty;
            }
            else
            {
                sceneObjectProperty = sceneObject.GameObject.AddComponent(trainingProperty) as ISceneObjectProperty;
            }

            return sceneObjectProperty;
        }

        /// <summary>
        /// Removes type of <paramref name="trainingProperty"/> from this <see cref="ISceneObject"/>.
        /// </summary>
        /// <param name="sceneObject"><see cref="ISceneObject"/> from whom the <paramref name="trainingProperty"/> will be removed.</param>
        /// <param name="trainingProperty"><see cref="ISceneObjectProperty"/> to be removed from <paramref name="sceneObject"/>.</param>
        /// <param name="removeDependencies">If true, this method also removes other components that are marked as `RequiredComponent` by <paramref name="trainingProperty"/>.</param>
        /// <param name="excludedFromBeingRemoved">The training properties in this list will not be removed if any is a dependency of <paramref name="trainingProperty"/>. Only relevant if <paramref name="removeDependencies"/> is true.</param>
        public static void RemoveTrainingProperty(this ISceneObject sceneObject, Component trainingProperty, bool removeDependencies = false, IEnumerable<Component> excludedFromBeingRemoved = null)
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
        /// <param name="excludedFromBeingRemoved">The training properties in this list will not be removed if any is a dependency of <paramref name="trainingProperty"/>. Only relevant if <paramref name="removeDependencies"/> is true.</param>
        public static void RemoveTrainingProperty(this ISceneObject sceneObject, Type trainingProperty, bool removeDependencies = false, IEnumerable<Component> excludedFromBeingRemoved = null)
        {
            Component trainingComponent = sceneObject.GameObject.GetComponent(trainingProperty);

            if (AreParametersNullOrInvalid(sceneObject, trainingProperty) || trainingComponent == null)
            {
                return;
            }

            IEnumerable<Type> typesToIgnore = GetTypesFromComponents(excludedFromBeingRemoved);
            RemoveProperty(sceneObject, trainingProperty, removeDependencies, typesToIgnore);
        }

        private static void RemoveProperty(ISceneObject sceneObject, Type typeToRemove, bool removeDependencies, IEnumerable<Type> typesToIgnore)
        {
            IEnumerable<Component> trainingProperties = sceneObject.GameObject.GetComponents(typeof(Component)).Where(component => component.GetType() != typeToRemove);

            foreach (Component component in trainingProperties)
            {
                if (IsTypeDependencyOfComponent(typeToRemove, component))
                {
                    RemoveProperty(sceneObject, component.GetType(), removeDependencies, typesToIgnore);
                }
            }

            Component trainingComponent = sceneObject.GameObject.GetComponent(typeToRemove);

#if UNITY_EDITOR
            Object.DestroyImmediate(trainingComponent);
#else
            Object.Destroy(trainingComponent);
#endif

            if (removeDependencies)
            {
                HashSet<Type> dependencies = GetAllDependenciesFrom(typeToRemove);

                if (dependencies == null)
                {
                    return;
                }

                // Some Unity native components like Rigidbody require Transform but Transform can't be removed.
                dependencies.Remove(typeof(Transform));

                foreach (Type dependency in dependencies.Except(typesToIgnore))
                {
                    RemoveProperty(sceneObject, dependency, removeDependencies, typesToIgnore);
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

        private static HashSet<Type> GetAllDependenciesFrom(Type trainingProperty)
        {
            RequireComponent[] requireComponents = trainingProperty.GetCustomAttributes(typeof(RequireComponent), false) as RequireComponent[];

            if (requireComponents == null || requireComponents.Length == 0)
            {
                return null;
            }

            HashSet<Type> dependencies = new HashSet<Type>();

            foreach (RequireComponent requireComponent in requireComponents)
            {
                AddTypeToList(requireComponent.m_Type0, ref dependencies);
                AddTypeToList(requireComponent.m_Type1, ref dependencies);
                AddTypeToList(requireComponent.m_Type2, ref dependencies);
            }

            return dependencies;
        }

        private static void AddTypeToList(Type type, ref HashSet<Type> dependencies)
        {
            if (type != null)
            {
                dependencies.Add(type);
            }
        }

        private static IEnumerable<Type> GetTypesFromComponents(IEnumerable<Component> components)
        {
            return components == null ? new Type[0] : components.Select(component => component.GetType());
        }
    }
}
