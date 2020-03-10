using NUnit.Framework;
using System.Collections;
using Innoactive.Hub.Training;
using Innoactive.Hub.Training.Conditions;
using Innoactive.Hub.Training.SceneObjects;
using Innoactive.Hub.Training.SceneObjects.Properties;
using UnityEngine;
using UnityEngine.TestTools;

namespace Innoactive.Creator.Core.Tests.Conditions
{
    [TestFixture]
    public class ObjectInColliderConditionTests : ObjectInTargetTestBase
    {
        [SetUp]
        public void SetUpColliderSceneObject()
        {
            // Setup collider training scene object
            BoxCollider boxCollider = TargetPositionObject.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            TargetPositionObject.AddComponent<ColliderWithTriggerProperty>();
            TargetTrainingSceneObject = TargetPositionObject.AddComponent<TrainingSceneObject>();
        }

        [SetUp]
        public void SetUpTrackedSceneObject()
        {
            // Setup tracked training scene object
            TrackedObject.AddComponent<BoxCollider>();
            Rigidbody rigidbody = TrackedObject.AddComponent<Rigidbody>();
            rigidbody.isKinematic = true;
            TrackedTrainingSceneObject = TrackedObject.AddComponent<TrainingSceneObject>();
        }

        [UnityTest]
        public IEnumerator CompleteWhenTargetObjectIsAtExactPositionAsCollider()
        {
            // Activate collider condition
            ObjectInColliderCondition condition = new ObjectInColliderCondition(TargetTrainingSceneObject.GetProperty<ColliderWithTriggerProperty>(), TrackedTrainingSceneObject);
            condition.LifeCycle.Activate();

            yield return null;
            condition.Update();

            // Move tracked object to the target position
            TrackedObject.transform.position = TargetPositionObject.transform.position;
            yield return null;
            condition.Update();
            yield return null;
            condition.Update();

            // Assert that condition is now completed
            Assert.IsTrue(condition.IsCompleted, "TargetInColliderCondition should be completed!");
        }

        [UnityTest]
        public IEnumerator CompleteWhenTargetObjectIsInsideCollider()
        {
            // Activate collider condition
            ObjectInColliderCondition condition = new ObjectInColliderCondition(TargetTrainingSceneObject.GetProperty<ColliderWithTriggerProperty>(), TrackedTrainingSceneObject);
            condition.LifeCycle.Activate();

            yield return null;
            condition.Update();

            // Move tracked object to the target position
            TrackedObject.transform.position = TargetPositionObject.transform.position - PositionOffsetNearTarget;
            yield return null;
            condition.Update();
            yield return null;
            condition.Update();

            // Assert that condition is now completed
            Assert.IsTrue(condition.IsCompleted, "TargetInColliderCondition should be completed!");
        }

        [UnityTest]
        public IEnumerator CompleteWhenTargetObjectIsAtExactPositionAsColliderOnStart()
        {
            // Move tracked object at the target position
            TrackedObject.transform.position = TargetPositionObject.transform.position;
            yield return null;

            // Activate collider condition
            ObjectInColliderCondition condition = new ObjectInColliderCondition(TargetTrainingSceneObject.GetProperty<ColliderWithTriggerProperty>(), TrackedTrainingSceneObject);
            condition.LifeCycle.Activate();

            while (condition.IsCompleted == false)
            {
                yield return null;
                condition.Update();
            }

            // Assert that condition is now completed
            Assert.IsTrue(condition.IsCompleted, "TargetInColliderCondition should be completed!");
        }

        [UnityTest]
        public IEnumerator CompleteWhenTargetObjectIsInsideColliderOnStart()
        {
            // Move tracked object at the target position
            TrackedObject.transform.position = TargetPositionObject.transform.position - PositionOffsetNearTarget;

            yield return null;

            // Activate collider condition
            ObjectInColliderCondition condition = new ObjectInColliderCondition(TargetTrainingSceneObject.GetProperty<ColliderWithTriggerProperty>(), TrackedTrainingSceneObject);
            condition.LifeCycle.Activate();

            while (condition.IsCompleted == false)
            {
                yield return null;
                condition.Update();
            }

            // Assert that condition is now completed
            Assert.IsTrue(condition.IsCompleted, "TargetInColliderCondition should be completed!");
        }

        [UnityTest]
        public IEnumerator CompleteWhenTargetObjectIsInsideColliderWithDuration()
        {
            // Set the target duration
            const float targetDuration = 0.1f;

            // Activate collider condition
            ObjectInColliderCondition condition = new ObjectInColliderCondition(TargetTrainingSceneObject.GetProperty<ColliderWithTriggerProperty>(), TrackedTrainingSceneObject, targetDuration);
            condition.LifeCycle.Activate();

            while (condition.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                condition.Update();
            }

            // Move tracked object to the target position
            TrackedObject.transform.position = TargetPositionObject.transform.position;

            Assert.IsFalse(condition.IsCompleted);

            yield return null;
            condition.Update();

            float startTime = Time.time;
            while (condition.IsCompleted == false)
            {
                yield return null;
                condition.Update();
            }

            float duration = Time.time - startTime;

            // Assert that condition is completed after the specified time.
            Assert.AreEqual(targetDuration, duration, Time.deltaTime);
            Assert.IsTrue(condition.IsCompleted, "TargetInColliderCondition should be completed!");
        }

        [UnityTest]
        public IEnumerator DontCompleteWhenTargetObjectLeavesColliderEarly()
        {
            // Set the target duration
            const float targetDuration = 0.1f;

            // Activate collider condition
            ObjectInColliderCondition condition = new ObjectInColliderCondition(TargetTrainingSceneObject.GetProperty<ColliderWithTriggerProperty>(), TrackedTrainingSceneObject, targetDuration);
            condition.LifeCycle.Activate();

            while (condition.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                condition.Update();
            }

            // Move tracked object to the target position
            TrackedObject.transform.position = TargetPositionObject.transform.position;

            float startTime = Time.time;
            while (Time.time < startTime + targetDuration * 0.3f)
            {
                yield return null;
                condition.Update();
            }

            // Move tracked object away from target position before condition is completed
            TrackedObject.transform.position = PositionFarFromTarget;

            startTime = Time.time;
            while (Time.time < startTime + targetDuration * 0.8f)
            {
                yield return null;
                condition.Update();
            }

            // Assert that condition is not completed
            Assert.IsFalse(condition.IsCompleted, "TargetInColliderCondition should not be completed!");
        }

        [UnityTest]
        public IEnumerator NotCompleted()
        {
            // Activate collider condition
            ObjectInColliderCondition condition = new ObjectInColliderCondition(TargetTrainingSceneObject.GetProperty<ColliderWithTriggerProperty>(), TrackedTrainingSceneObject);
            condition.LifeCycle.Activate();

            while (condition.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                condition.Update();
            }

            // Assert that condition is not completed
            Assert.IsFalse(condition.IsCompleted, "TargetInColliderCondition should not be completed!");
        }

        [UnityTest]
        public IEnumerator DontCompleteWhenWrongObjectEntersCollider()
        {
            // In addition to setup phase, also setup an additional object
            GameObject wrongObj = new GameObject("Wrong Object");
            wrongObj.transform.position = PositionFarFromTarget;
            wrongObj.AddComponent<BoxCollider>();
            wrongObj.AddComponent<Rigidbody>();
            TrainingSceneObject wrongTrainingSceneObject = wrongObj.AddComponent<TrainingSceneObject>();

            // Activate collider condition
            ObjectInColliderCondition condition = new ObjectInColliderCondition(TargetTrainingSceneObject.GetProperty<ColliderWithTriggerProperty>(), TrackedTrainingSceneObject);
            condition.LifeCycle.Activate();

            while (condition.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                condition.Update();
            }

            // Move tracked object to the target position
            wrongTrainingSceneObject.transform.position = TargetPositionObject.transform.position;

            float startTime = Time.time;
            while (Time.time < startTime + 0.1f)
            {
                yield return null;
                condition.Update();
            }

            // Assert that condition is not completed
            Assert.IsFalse(condition.IsCompleted, "TargetInColliderCondition should not be completed!");
        }

        [UnityTest]
        public IEnumerator AutoCompleteActive()
        {
            // Given an object in an activated collider condition,
            ObjectInColliderCondition condition = new ObjectInColliderCondition(TargetTrainingSceneObject.GetProperty<ColliderWithTriggerProperty>(), TrackedTrainingSceneObject);

            bool isColliding = false;
            TargetTrainingSceneObject.GetProperty<ColliderWithTriggerProperty>().EnteredTrigger += (sender, args) => isColliding = true;

            condition.LifeCycle.Activate();

            while (condition.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                condition.Update();
            }

            // When you autocomplete it,
            condition.Autocomplete();

            // Then condition is activated and the object is moved into collider.
            Assert.AreEqual(Stage.Active, condition.LifeCycle.Stage);
            Assert.IsTrue(isColliding);
            Assert.IsTrue(condition.IsCompleted);
            yield break;
        }

        [UnityTest]
        public IEnumerator FastForwardDoesNotCompleteCondition()
        {
            // Given an activated object in collider condition,
            ObjectInColliderCondition condition = new ObjectInColliderCondition(TargetTrainingSceneObject.GetProperty<ColliderWithTriggerProperty>(), TrackedTrainingSceneObject);

            bool isColliding = false;
            TargetTrainingSceneObject.GetProperty<ColliderWithTriggerProperty>().EnteredTrigger += (sender, args) => isColliding = true;

            condition.LifeCycle.Activate();

            while (condition.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                condition.Update();
            }

            // When you fast-forward it
            condition.LifeCycle.MarkToFastForward();

            // Then nothing happens.
            Assert.AreEqual(Stage.Active, condition.LifeCycle.Stage);
            Assert.IsFalse(condition.IsCompleted);
            Assert.IsFalse(isColliding);
        }
    }
}
