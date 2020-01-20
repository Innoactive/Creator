#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Common.Logging;
using NUnit.Framework;
using UnityEngine.TestTools;
using Innoactive.Hub.Training.Utils;
using Innoactive.Hub.Training.SceneObjects;
using Innoactive.Hub.Training.SceneObjects.Properties;

namespace Innoactive.Hub.Unity.Tests.Training
{
    public class TrainingPropertyTests : RuntimeTests
    {
        private static readonly ILog logger = LogManager.GetLogger<TrainingPropertyTests>();

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
            // Required for ColliderWithTriggerProperty
            SceneObject.GameObject.AddComponent<BoxCollider>().isTrigger = true;

            foreach (Type propertyType in TrainingProperties)
            {
                SceneObject.AddTrainingProperty(propertyType);

                yield return null;

                Assert.That(SceneObject.GameObject.GetComponent(propertyType));
            }

            int totalOfPublicProperties = TrainingProperties.Count();
            int totalOfAddedProperties = SceneObject.Properties.Count;

            Assert.AreEqual(totalOfAddedProperties, totalOfPublicProperties);
        }

        [UnityTest]
        public IEnumerator AddAndRemoveTrainingProperties()
        {
            // Required for ColliderWithTriggerProperty
            SceneObject.GameObject.AddComponent<BoxCollider>().isTrigger = true;

            yield return AddTrainingProperties();

            foreach (Component propertyComponent in SceneObject.GameObject.GetComponents(typeof(TrainingSceneObjectProperty)))
            {
                SceneObject.RemoveTrainingProperty(propertyComponent);

                yield return null;

                Assert.That(SceneObject.GameObject.GetComponent(propertyComponent.GetType()) == null);
            }

            int totalOfAddedProperties = SceneObject.Properties.Count;

            Assert.AreEqual(0, totalOfAddedProperties);
        }

        [UnityTest]
        public IEnumerator AddPropertyWithDependencies()
        {
            List<Type> dependencies = new List<Type>();
            Type trainingProperty = TrainingProperties.First(propertyType => GetAllDependenciesFrom(propertyType, ref dependencies));

            if (trainingProperty == null)
            {
                logger.WarnFormat("AddPropertyWithItsDependencies from {0} was ignored because no TrainingProperties with dependencies could be found.", GetType().Name);
                Assert.Ignore();
            }

            yield return AddPropertyAndVerifyDependencies(trainingProperty, dependencies);
        }

        [UnityTest]
        public IEnumerator RemovePropertyAndDependencies()
        {
            List<Type> dependencies = new List<Type>();
            Type trainingProperty = TrainingProperties.First(propertyType => GetAllDependenciesFrom(propertyType, ref dependencies));

            if (trainingProperty == null)
            {
                logger.WarnFormat("RemovePropertyAndDependencies from {0} was ignored because no TrainingProperties with dependencies could be found.", GetType().Name);
                Assert.Ignore();
            }

            yield return AddPropertyAndVerifyDependencies(trainingProperty, dependencies);

            SceneObject.RemoveTrainingProperty(trainingProperty, true);

            yield return null;

            Assert.That(SceneObject.GameObject.GetComponent(trainingProperty) == null);

            foreach (Type dependency in dependencies)
            {
                Assert.That(SceneObject.GameObject.GetComponent(dependency) == null);
            }
        }

        [UnityTest]
        public IEnumerator AddPropertyAndRemoveDependency()
        {
            List<Type> dependencies = new List<Type>();
            Type trainingProperty = TrainingProperties.First(propertyType => GetAllDependenciesFrom(propertyType, ref dependencies, true)), dependantType = dependencies.First();

            if (trainingProperty == null || dependantType == null)
            {
                logger.WarnFormat("AddPropertyAndRemoveDependency from {0} was ignored because no TrainingProperties with dependencies as ISceneObjectProperty could be found.", GetType().Name);
                Assert.Ignore();
            }

            SceneObject.AddTrainingProperty(trainingProperty);

            yield return null;

            Assert.That(SceneObject.GameObject.GetComponent(trainingProperty));
            Assert.That(SceneObject.GameObject.GetComponent(dependantType));

            SceneObject.RemoveTrainingProperty(dependantType);

            yield return null;

            Assert.That(SceneObject.GameObject.GetComponent(trainingProperty) == null);
            Assert.That(SceneObject.GameObject.GetComponent(dependantType) == null);
        }

        [UnityTest]
        public IEnumerator RemovePropertyWithoutDependencies()
        {
            List<Type> dependencies = new List<Type>();
            Type trainingProperty = TrainingProperties.First(propertyType => GetAllDependenciesFrom(propertyType, ref dependencies));

            if (trainingProperty == null)
            {
                logger.WarnFormat("RemovePropertyAndDependencies from {0} was ignored because no TrainingProperties with dependencies could be found.", GetType().Name);
                Assert.Ignore();
            }

            yield return AddPropertyAndVerifyDependencies(trainingProperty, dependencies);

            // Parameter removeDependencies is automatically initialized as false.
            SceneObject.RemoveTrainingProperty(trainingProperty);

            yield return null;

            Assert.That(SceneObject.GameObject.GetComponent(trainingProperty) == null);

            foreach (Type dependency in dependencies)
            {
                Assert.That(SceneObject.GameObject.GetComponent(dependency));
            }
        }

        [UnityTest]
        public IEnumerator TryToAddNotTrainingProperties()
        {
            foreach (Type notAPropertyType in NotTrainingProperties)
            {
                SceneObject.AddTrainingProperty(notAPropertyType);

                yield return null;

                Assert.That(SceneObject.GameObject.GetComponent(notAPropertyType) == null);
            }
        }

        private IEnumerator AddPropertyAndVerifyDependencies(Type trainingProperty, List<Type> dependencies)
        {
            SceneObject.AddTrainingProperty(trainingProperty);

            Assert.That(SceneObject.GameObject.GetComponent(trainingProperty));

            yield return null;

            foreach (Type dependency in dependencies)
            {
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

#endif
