using NUnit.Framework;
using System.Collections;
using Innoactive.Creator.Core.Tests.Utils;
using Innoactive.Hub.Training.Configuration;
using Innoactive.Hub.Training.Exceptions;
using Innoactive.Hub.Training.SceneObjects;
using UnityEngine;
using UnityEngine.TestTools;

namespace Innoactive.Creator.Core.Tests
{
    public class SceneObjectRegistryTests : RuntimeTests
    {
        [UnityTest]
        public IEnumerator IsRegisteredAtRegistryTest()
        {
            // Create reference
            GameObject obj = new GameObject("MyObject");
            TrainingSceneObject reference = obj.AddComponent<TrainingSceneObject>();

            // Await end of frame
            yield return new WaitForFixedUpdate();

            // Assert that reference is now registered at the registry.
            Assert.IsTrue(RuntimeConfigurator.Configuration.SceneObjectRegistry.ContainsGuid(reference.Guid));
            Assert.AreEqual(reference, RuntimeConfigurator.Configuration.SceneObjectRegistry.GetByGuid(reference.Guid));

            // Clean up
            Object.DestroyImmediate(obj);
        }

        [UnityTest]
        public IEnumerator ChangeNameAfterAwakeTest()
        {
            // Create reference
            GameObject obj = new GameObject("MyObject");
            obj.SetActive(false);
            TrainingSceneObject reference = obj.AddComponent<TrainingSceneObject>();
            reference.ChangeUniqueName("Test");
            obj.SetActive(true);

            reference.ChangeUniqueName("Test2");

            // Assert that reference is now registered at the registry.
            Assert.IsTrue(RuntimeConfigurator.Configuration.SceneObjectRegistry.ContainsGuid(reference.Guid));
            Assert.AreEqual(reference, RuntimeConfigurator.Configuration.SceneObjectRegistry.GetByGuid(reference.Guid));
            Assert.IsFalse(RuntimeConfigurator.Configuration.SceneObjectRegistry.ContainsName("Test"));
            Assert.IsTrue(RuntimeConfigurator.Configuration.SceneObjectRegistry.ContainsName("Test2"));
            Assert.AreEqual(reference, RuntimeConfigurator.Configuration.SceneObjectRegistry.GetByName("Test2"));

            // Clean up
            Object.DestroyImmediate(obj);

            yield return null;
        }

        [UnityTest]
        public IEnumerator ChangeNameBeforeAwakeTest()
        {
            // Create reference
            GameObject obj = new GameObject("MyObject");
            obj.SetActive(false);
            TrainingSceneObject reference = obj.AddComponent<TrainingSceneObject>();
            reference.ChangeUniqueName("Test");

            reference.ChangeUniqueName("Test2");
            obj.SetActive(true);

            // Assert that reference is now registered at the registry.
            Assert.IsTrue(RuntimeConfigurator.Configuration.SceneObjectRegistry.ContainsGuid(reference.Guid));
            Assert.AreEqual(reference, RuntimeConfigurator.Configuration.SceneObjectRegistry.GetByGuid(reference.Guid));
            Assert.IsFalse(RuntimeConfigurator.Configuration.SceneObjectRegistry.ContainsName("Test"));
            Assert.IsTrue(RuntimeConfigurator.Configuration.SceneObjectRegistry.ContainsName("Test2"));
            Assert.AreEqual(reference, RuntimeConfigurator.Configuration.SceneObjectRegistry.GetByName("Test2"));

            // Clean up
            Object.DestroyImmediate(obj);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CanBeFoundByUniqueNameTest()
        {
            // Create reference
            GameObject obj = new GameObject("MyObject");
            TrainingSceneObject reference = obj.AddComponent<TrainingSceneObject>();
            reference.ChangeUniqueName("Test");

            // Await end of frame
            yield return new WaitForFixedUpdate();
            // Assert that reference is now registered at the registry.
            Assert.IsTrue(RuntimeConfigurator.Configuration.SceneObjectRegistry.ContainsName(reference.UniqueName));
            Assert.AreEqual(reference, RuntimeConfigurator.Configuration.SceneObjectRegistry.GetByName(reference.UniqueName));

            // Clean up
            Object.DestroyImmediate(obj);
        }

        [UnityTest]
        public IEnumerator UnregisterRemovesReferenceTest()
        {
            // Create reference
            GameObject obj = new GameObject("MyObject");
            TrainingSceneObject reference = obj.AddComponent<TrainingSceneObject>();
            reference.ChangeUniqueName("Test");

            // Await end of frame
            yield return new WaitForFixedUpdate();

            RuntimeConfigurator.Configuration.SceneObjectRegistry.Unregister(reference);
            // Assert that you cant find reference by guid or name
            Assert.IsFalse(RuntimeConfigurator.Configuration.SceneObjectRegistry.ContainsGuid(reference.Guid));
            Assert.IsFalse(RuntimeConfigurator.Configuration.SceneObjectRegistry.ContainsName(reference.UniqueName));

            // Clean up
            Object.DestroyImmediate(obj);
        }

        [UnityTest]
        public IEnumerator UnregisterAllowsToPlaceTest()
        {
            // Create reference
            GameObject obj1 = new GameObject("MyObject");
            TrainingSceneObject reference1 = obj1.AddComponent<TrainingSceneObject>();
            reference1.ChangeUniqueName("Test");

            RuntimeConfigurator.Configuration.SceneObjectRegistry.Unregister(reference1);

            // Create reference
            GameObject obj2 = new GameObject("MyObject");
            TrainingSceneObject reference2 = obj2.AddComponent<TrainingSceneObject>();
            reference2.ChangeUniqueName("Test");

            // Assert that new added reference can be found
            Assert.IsTrue(RuntimeConfigurator.Configuration.SceneObjectRegistry.ContainsGuid(reference2.Guid));
            Assert.IsTrue(RuntimeConfigurator.Configuration.SceneObjectRegistry.ContainsName(reference2.UniqueName));
            Assert.AreEqual(reference2, RuntimeConfigurator.Configuration.SceneObjectRegistry.GetByName(reference2.UniqueName));

            // Clean up
            Object.DestroyImmediate(obj1);
            Object.DestroyImmediate(obj2);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CantBeRegisteredTwiceTest()
        {
            // Create reference
            GameObject obj = new GameObject("MyObject");
            TrainingSceneObject reference = obj.AddComponent<TrainingSceneObject>();
            reference.ChangeUniqueName("Test");

            // Assert that exception is thrown
            Assert.Throws(typeof(AlreadyRegisteredException),
                () =>
                {
                    RuntimeConfigurator.Configuration.SceneObjectRegistry.Register(reference);
                },
                "ReferenceAlreadyRegisteredException was not thrown!");

            // Clean up
            Object.DestroyImmediate(obj);

            yield return null;
        }

        [UnityTest]
        public IEnumerator UnregisterOnDestroyTest()
        {
            // Create reference
            GameObject obj = new GameObject("MyObject");
            TrainingSceneObject reference = obj.AddComponent<TrainingSceneObject>();
            reference.ChangeUniqueName("Test");

            Object.DestroyImmediate(obj);

            // Assert that exception is thrown
            Assert.IsFalse(RuntimeConfigurator.Configuration.SceneObjectRegistry.ContainsName("Test"));

            yield return null;
        }

        [UnityTest]
        public IEnumerator IgnoreNotUniqueName()
        {
            // Ignore log messages for this test.
            LogAssert.ignoreFailingMessages = true;

            // Given two references,
            GameObject obj = new GameObject("MyObject");
            obj.SetActive(false);
            TrainingSceneObject reference1 = obj.AddComponent<TrainingSceneObject>();
            reference1.ChangeUniqueName("Test");
            TrainingSceneObject reference2 = obj.AddComponent<TrainingSceneObject>();

            // When we change the unique name of the second reference to the unique name of the first reference,
            reference2.ChangeUniqueName("Test");
            obj.SetActive(true);

            // Then the second reference is not renamed to 'Test' and the first reference keeps its name.
            Assert.AreEqual("Test", reference1.UniqueName);
            Assert.AreNotEqual("Test", reference2.UniqueName);

            yield return null;
        }
    }
}
