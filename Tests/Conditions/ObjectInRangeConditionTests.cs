using NUnit.Framework;
using System.Collections;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Conditions;
using Innoactive.Creator.Core.SceneObjects;
using Innoactive.Creator.Core.SceneObjects.Properties;
using UnityEngine;
using UnityEngine.TestTools;

namespace Innoactive.Creator.Tests.Conditions
{
    [TestFixture]
    public class ObjectInRangeConditionTests : ObjectInTargetTestBase
    {
        [SetUp]
        public void SetUpRangeSceneObject()
        {
            // Setup collider training scene object
            TargetTrainingSceneObject = TargetPositionObject.AddComponent<TrainingSceneObject>();
            TargetPositionObject.AddComponent<TransformInRangeDetectorProperty>();
        }

        [SetUp]
        public void SetUpTrackedSceneObject()
        {
            // Setup tracked training scene object
            TrackedTrainingSceneObject = TrackedObject.AddComponent<TrainingSceneObject>();
        }

        [UnityTest]
        public IEnumerator CompleteWhenTargetObjectIsAtZeroRange()
        {
            // Activate in range condition
            ObjectInRangeCondition condition = new ObjectInRangeCondition(TrackedTrainingSceneObject, TargetTrainingSceneObject.GetProperty<TransformInRangeDetectorProperty>(), 5);
            condition.LifeCycle.Activate();

            while (condition.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                condition.Update();
            }

            // Move tracked object to the target position
            TrackedObject.transform.position = TargetPositionObject.transform.position;

            while (condition.IsCompleted == false)
            {
                yield return null;
                condition.Update();
            }

            // Assert that condition is now completed
            UnityEngine.Assertions.Assert.IsTrue(condition.IsCompleted, "TargetInRangeCondition should be completed!");
        }

        [UnityTest]
        public IEnumerator CompleteWhenTargetObjectIsInsideRange()
        {
            // Activate in range condition
            ObjectInRangeCondition condition = new ObjectInRangeCondition(TrackedTrainingSceneObject, TargetTrainingSceneObject.GetProperty<TransformInRangeDetectorProperty>(), 5);
            condition.LifeCycle.Activate();

            while (condition.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                condition.Update();
            }

            // Move tracked object to the target position
            TrackedObject.transform.position = TargetPositionObject.transform.position - PositionOffsetNearTarget;

            while (condition.IsCompleted == false)
            {
                yield return null;
                condition.Update();
            }

            // Assert that condition is now completed
            UnityEngine.Assertions.Assert.IsTrue(condition.IsCompleted, "TargetInRangeCondition should be completed!");
        }

        [UnityTest]
        public IEnumerator CompleteWhenTargetObjectIsAtZeroRangeOnStart()
        {
            // Move tracked object at the target position
            TrackedObject.transform.position = TargetPositionObject.transform.position;
            yield return null;

            // Activate in range condition
            ObjectInRangeCondition condition = new ObjectInRangeCondition(TrackedTrainingSceneObject, TargetTrainingSceneObject.GetProperty<TransformInRangeDetectorProperty>(), 5);
            condition.LifeCycle.Activate();

            while (condition.IsCompleted == false)
            {
                yield return null;
                condition.Update();
            }

            // Assert that condition is now completed
            UnityEngine.Assertions.Assert.IsTrue(condition.IsCompleted, "TargetInRangeCondition should be completed!");
        }

        [UnityTest]
        public IEnumerator CompleteWhenTargetObjectIsInsideRangeOnStart()
        {
            // Move tracked object at the target position
            TrackedObject.transform.position = TargetPositionObject.transform.position - PositionOffsetNearTarget;
            yield return null;

            // Activate in range condition
            ObjectInRangeCondition condition = new ObjectInRangeCondition(TrackedTrainingSceneObject, TargetTrainingSceneObject.GetProperty<TransformInRangeDetectorProperty>(), 5);
            condition.LifeCycle.Activate();

            while (condition.IsCompleted == false)
            {
                yield return null;
                condition.Update();
            }

            // Assert that condition is now completed
            UnityEngine.Assertions.Assert.IsTrue(condition.IsCompleted, "TargetInRangeCondition should be completed!");
        }

        [UnityTest]
        public IEnumerator CompleteWhenTargetObjectIsAtZeroRangeWithDuration()
        {
            // Set the target duration
            const float targetDuration = 0.1f;

            // Activate in range condition
            ObjectInRangeCondition condition = new ObjectInRangeCondition(TrackedTrainingSceneObject, TargetTrainingSceneObject.GetProperty<TransformInRangeDetectorProperty>(), 5, targetDuration);
            condition.LifeCycle.Activate();

            while (condition.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                condition.Update();
            }

            // Move tracked object to the target position
            TrackedObject.transform.position = TargetPositionObject.transform.position;

            yield return null;
            condition.Update();

            float startTime = Time.time;
            while (condition.IsCompleted == false)
            {
                yield return null;
                condition.Update();
            }

            float duration = Time.time - startTime;

            // Assert that condition has been completed after the specified duration
            Assert.AreEqual(targetDuration, duration, Time.deltaTime);
            Assert.IsTrue(condition.IsCompleted, "TargetInRangeCondition should be completed!");
        }

        [UnityTest]
        public IEnumerator CompleteWhenTargetObjectIsInsideRangeWithDuration()
        {
            // Set the target duration
            const float targetDuration = 0.1f;

            // Activate in range condition
            ObjectInRangeCondition condition = new ObjectInRangeCondition(TrackedTrainingSceneObject, TargetTrainingSceneObject.GetProperty<TransformInRangeDetectorProperty>(), 5, targetDuration);
            condition.LifeCycle.Activate();

            while (condition.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                condition.Update();
            }

            // Move tracked object to the target position
            TrackedObject.transform.position = TargetPositionObject.transform.position - PositionOffsetNearTarget;

            yield return null;
            condition.Update();

            float startTime = Time.time;
            while (condition.IsCompleted == false)
            {
                yield return null;
                condition.Update();
            }

            float duration = Time.time - startTime;

            // Assert that condition has been completed after the specified duration
            Assert.AreEqual(targetDuration, duration, 0.05f);
            UnityEngine.Assertions.Assert.IsTrue(condition.IsCompleted, "TargetInRangeCondition should be completed!");
        }

        [UnityTest]
        public IEnumerator DontCompleteWhenTargetObjectLeavesRangeEarly()
        {
            // Set the target duration
            const float targetDuration = 0.1f;

            // Activate range condition
            ObjectInRangeCondition condition = new ObjectInRangeCondition(TrackedTrainingSceneObject, TargetTrainingSceneObject.GetProperty<TransformInRangeDetectorProperty>(), 5, targetDuration);
            condition.LifeCycle.Activate();

            while (condition.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                condition.Update();
            }

            // Move tracked object to the target position
            TrackedObject.transform.position = TargetPositionObject.transform.position;

            float startTime = Time.time;
            while (Time.time < startTime + 0.3f * targetDuration)
            {
                yield return null;
                condition.Update();
            }

            // Move tracked object away from target position before condition is completed
            TrackedObject.transform.position = PositionFarFromTarget;

            startTime = Time.time;
            while (Time.time < startTime + 0.8f * targetDuration)
            {
                yield return null;
                condition.Update();
            }

            // Assert that condition is not completed
            UnityEngine.Assertions.Assert.IsFalse(condition.IsCompleted, "TargetInRangeCondition should not be completed!");
        }

        [UnityTest]
        public IEnumerator NotCompletedTest()
        {
            // Activate range condition
            ObjectInRangeCondition condition = new ObjectInRangeCondition(TrackedTrainingSceneObject, TargetTrainingSceneObject.GetProperty<TransformInRangeDetectorProperty>(), 5);
            condition.LifeCycle.Activate();
            yield return null;

            // Assert that condition is not completed
            UnityEngine.Assertions.Assert.IsFalse(condition.IsCompleted, "TargetInRangeCondition should not be completed!");
        }

        [UnityTest]
        public IEnumerator DontCompleteWhenWrongObjectEntersRange()
        {
            // In addition to setup phase, also setup an additional object
            GameObject wrongObj = new GameObject("Wrong Object");
            wrongObj.transform.position = PositionFarFromTarget;
            TrainingSceneObject wrongTrainingSceneObject = wrongObj.AddComponent<TrainingSceneObject>();

            // Activate range condition
            ObjectInRangeCondition condition = new ObjectInRangeCondition(TrackedTrainingSceneObject, TargetTrainingSceneObject.GetProperty<TransformInRangeDetectorProperty>(), 5);
            condition.LifeCycle.Activate();

            while (condition.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                condition.Update();
            }

            // Move wrong object to the target position
            wrongTrainingSceneObject.transform.position = TargetPositionObject.transform.position;

            float startTime = Time.time;
            while (Time.time < startTime + 0.1f)
            {
                yield return null;
                condition.Update();
            }

            // Assert that condition is not completed
            UnityEngine.Assertions.Assert.IsFalse(condition.IsCompleted, "TargetInRangeCondition should not be completed!");
        }

        [UnityTest]
        public IEnumerator AutoCompleteActive()
        {
            // Given an object in range condition,
            ObjectInRangeCondition condition = new ObjectInRangeCondition(TrackedTrainingSceneObject, TargetTrainingSceneObject.GetProperty<TransformInRangeDetectorProperty>(), 5);

            // When you activate and autocomplete it,
            condition.LifeCycle.Activate();

            while (condition.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                condition.Update();
            }

            condition.Autocomplete();

            // Then the condition is complete and the object is moved
            Assert.AreEqual(Stage.Active, condition.LifeCycle.Stage);
            Assert.IsTrue(condition.IsCompleted);
            Assert.IsTrue(TrackedTrainingSceneObject.GameObject.transform.position == TargetTrainingSceneObject.GameObject.transform.position);
        }

        [UnityTest]
        public IEnumerator FastForwardDoesNotCompleteCondition()
        {
            // Given an object in range condition,
            ObjectInRangeCondition condition = new ObjectInRangeCondition(TrackedTrainingSceneObject, TargetTrainingSceneObject.GetProperty<TransformInRangeDetectorProperty>(), 5);

            // When you activate it,
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
            Assert.IsFalse(TrackedTrainingSceneObject.GameObject.transform.position == TargetTrainingSceneObject.GameObject.transform.position);
        }
    }
}
