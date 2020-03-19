using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Innoactive.Creator.Core.SceneObjects;
using Innoactive.Creator.Core.Properties;
using Innoactive.Creator.Core.Utils;
using Innoactive.Creator.Tests.Utils;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Innoactive.Creator.Tests.Properties
{
    public class TrainingPropertyTests : RuntimeTests
    {
        public static IEnumerable<Type> TrainingProperties
        {
            get { return ReflectionUtils.GetConcreteImplementationsOf(typeof(TrainingSceneObjectProperty)).Where(type => type.IsPublic); }
        }

        public static readonly IEnumerable<Type> NotTrainingProperties = new Type[]
        {
            typeof(Rigidbody),
            typeof(BoxCollider),
            typeof(Camera),
            typeof(Light),
            typeof(UnityEngine.Video.VideoPlayer),
            typeof(UnityEngine.UI.Button)
        };

        protected ISceneObject SceneObject;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            GameObject gameObject = new GameObject("Scene Object");
            SceneObject = gameObject.AddComponent<TrainingSceneObject>();
        }

        [UnityTest]
        public IEnumerator AddTrainingProperties()
        {
            // Given a ISceneObject.

            // Required for ColliderWithTriggerProperty
            SceneObject.GameObject.AddComponent<BoxCollider>().isTrigger = true;

            foreach (Type propertyType in TrainingProperties)
            {
                // When adding the ISceneObjectProperty to the ISceneObject.
                SceneObject.AddTrainingProperty(propertyType);

                yield return null;

                // Then assert that the ISceneObjectProperty is part of ISceneObject.
                Assert.That(SceneObject.GameObject.GetComponent(propertyType));
            }

            int totalOfPublicProperties = TrainingProperties.Count();
            int totalOfAddedProperties = SceneObject.Properties.Count;

            // Then assert that the ISceneObject.Properties considers all the ISceneObjectProperty added to ISceneObject.
            Assert.AreEqual(totalOfAddedProperties, totalOfPublicProperties);
        }

        [UnityTest]
        public IEnumerator AddAndRemoveTrainingProperties()
        {
            // Given a ISceneObject.

            // Required for ColliderWithTriggerProperty
            SceneObject.GameObject.AddComponent<BoxCollider>().isTrigger = true;

            // When adding a list of ISceneObjectProperty to the ISceneObject.
            yield return AddTrainingProperties();

            foreach (Component propertyComponent in SceneObject.GameObject.GetComponents(typeof(TrainingSceneObjectProperty)))
            {
                // When removing a ISceneObjectProperty from the ISceneObject.
                SceneObject.RemoveTrainingProperty(propertyComponent);

                yield return null;

                // Then assert that the ISceneObjectProperty is no longer part of ISceneObject.
                Assert.That(SceneObject.GameObject.GetComponent(propertyComponent.GetType()) == null);
            }

            int totalOfAddedProperties = SceneObject.Properties.Count;

            // Then assert that all ISceneObjectProperty were removed.
            Assert.AreEqual(0, totalOfAddedProperties);
        }

        [UnityTest]
        public IEnumerator AddPropertyWithDependencies()
        {
            // Given an ISceneObjectProperty and a list with its dependencies.
            List<Type> dependencies = new List<Type>();
            Type trainingProperty = TrainingProperties.First(propertyType => GetAllDependenciesFrom(propertyType, ref dependencies));

            if (trainingProperty == null)
            {
                Debug.LogWarningFormat("AddPropertyWithItsDependencies from {0} was ignored because no TrainingProperties with dependencies could be found.", GetType().Name);
                Assert.Ignore();
            }

            // When adding the ISceneObjectProperty.
            // Then assert that all its dependencies were added.
            yield return AddPropertyAndVerifyDependencies(trainingProperty, dependencies);
        }

        [UnityTest]
        public IEnumerator RemovePropertyAndDependencies()
        {
            // Given an ISceneObjectProperty and a list with its dependencies.
            List<Type> dependencies = new List<Type>();
            Type trainingProperty = TrainingProperties.First(propertyType => GetAllDependenciesFrom(propertyType, ref dependencies));

            if (trainingProperty == null)
            {
                Debug.LogWarningFormat("RemovePropertyAndDependencies from {0} was ignored because no TrainingProperties with dependencies could be found.", GetType().Name);
                Assert.Ignore();
            }

            // When adding the ISceneObjectProperty, we also make sure that all its dependencies were added.
            yield return AddPropertyAndVerifyDependencies(trainingProperty, dependencies);

            // When removing the ISceneObjectProperty forcing to remove its dependencies.
            SceneObject.RemoveTrainingProperty(trainingProperty, true);

            yield return null;

            // Then assert that the ISceneObjectProperty is not longer part of ISceneObject.
            Assert.That(SceneObject.GameObject.GetComponent(trainingProperty) == null);

            foreach (Type dependency in dependencies)
            {
                // Then assert that the dependencies of the ISceneObjectProperty are not longer part of ISceneObject.
                Assert.That(SceneObject.GameObject.GetComponent(dependency) == null);
            }
        }

        [UnityTest]
        public IEnumerator AddPropertyAndRemoveDependency()
        {
            // Given an ISceneObjectProperty, a list with its dependencies and a type dependant of ISceneObjectProperty.
            List<Type> dependencies = new List<Type>();
            Type trainingProperty = TrainingProperties.First(propertyType => GetAllDependenciesFrom(propertyType, ref dependencies, true)), dependantType = dependencies.First();

            if (trainingProperty == null || dependantType == null)
            {
                Debug.LogWarningFormat("AddPropertyAndRemoveDependency from {0} was ignored because no TrainingProperties with dependencies as ISceneObjectProperty could be found.", GetType().Name);
                Assert.Ignore();
            }

            // When adding adding the ISceneObjectProperty, we also make sure that all its dependencies were added.
            yield return AddPropertyAndVerifyDependencies(trainingProperty, dependencies);

            // Then assert that the ISceneObjectProperty and the property that depends of it are part of ISceneObject.
            Assert.That(SceneObject.GameObject.GetComponent(trainingProperty));
            Assert.That(SceneObject.GameObject.GetComponent(dependantType));

            // When removing the property that depends of ISceneObjectProperty.
            SceneObject.RemoveTrainingProperty(dependantType);

            yield return null;

            // Then assert that the ISceneObjectProperty and the property that depends of it are no longer part of ISceneObject.
            Assert.That(SceneObject.GameObject.GetComponent(trainingProperty) == null);
            Assert.That(SceneObject.GameObject.GetComponent(dependantType) == null);
        }

        [UnityTest]
        public IEnumerator RemovePropertyWithoutDependencies()
        {
            // Given an ISceneObjectProperty and a list with its dependencies.
            List<Type> dependencies = new List<Type>();
            Type trainingProperty = TrainingProperties.First(propertyType => GetAllDependenciesFrom(propertyType, ref dependencies));

            if (trainingProperty == null)
            {
                Debug.LogWarningFormat("RemovePropertyAndDependencies from {0} was ignored because no TrainingProperties with dependencies could be found.", GetType().Name);
                Assert.Ignore();
            }

            // When adding adding the ISceneObjectProperty, we also make sure that all its dependencies were added.
            yield return AddPropertyAndVerifyDependencies(trainingProperty, dependencies);

            // When removing the ISceneObjectProperty without forcing to remove its dependencies.
            // Parameter removeDependencies is automatically initialized as false.
            SceneObject.RemoveTrainingProperty(trainingProperty);

            yield return null;

            // Then assert that the ISceneObjectProperty is not longer part of ISceneObject.
            Assert.That(SceneObject.GameObject.GetComponent(trainingProperty) == null);

            foreach (Type dependency in dependencies)
            {
                // Then assert that the dependencies of the ISceneObjectProperty continuing in ISceneObject.
                Assert.That(SceneObject.GameObject.GetComponent(dependency));
            }
        }

        [UnityTest]
        public IEnumerator TryToAddNotTrainingProperties()
        {
            // Given an ISceneObject and a list of non-ISceneObjectProperty types.
            foreach (Type notAPropertyType in NotTrainingProperties)
            {
                // When trying to add a non-ISceneObjectProperty using ISceneObject.AddTrainingProperty.
                SceneObject.AddTrainingProperty(notAPropertyType);

                yield return null;

                // Then assert that the type was not attached to ISceneObject.
                Assert.That(SceneObject.GameObject.GetComponent(notAPropertyType) == null);
            }
        }

        private IEnumerator AddPropertyAndVerifyDependencies(Type trainingProperty, List<Type> dependencies)
        {
            // Given an ISceneObject.
            SceneObject.AddTrainingProperty(trainingProperty);

            // When adding an ISceneObjectProperty.
            Assert.That(SceneObject.GameObject.GetComponent(trainingProperty));

            yield return null;

            foreach (Type dependency in dependencies)
            {
                // Then assert that ISceneObject has all the dependencies required by the ISceneObjectProperty.
                Assert.That(SceneObject.GameObject.GetComponent(dependency));
            }
        }

        private bool GetAllDependenciesFrom(Type trainingProperty, ref List<Type> dependencies, bool onlyTrainingProperties = false)
        {
            RequireComponent[] requireComponents = trainingProperty.GetCustomAttributes(typeof(RequireComponent), false) as RequireComponent[];

            if (requireComponents == null || requireComponents.Length == 0)
            {
                return false;
            }

            foreach (RequireComponent requireComponent in requireComponents)
            {
                AddTypeToListIfNew(requireComponent.m_Type0, ref dependencies, onlyTrainingProperties);
                AddTypeToListIfNew(requireComponent.m_Type1, ref dependencies, onlyTrainingProperties);
                AddTypeToListIfNew(requireComponent.m_Type2, ref dependencies, onlyTrainingProperties);
            }

            return dependencies.Count > 0;
        }

        private void AddTypeToListIfNew(Type type, ref List<Type> dependencies, bool onlyTrainingProperties = false)
        {
            if (type != null && dependencies.Contains(type) == false)
            {
                if (onlyTrainingProperties && typeof(ISceneObjectProperty).IsAssignableFrom(type) == false)
                {
                    return;
                }

                dependencies.Add(type);
            }
        }
    }
}
