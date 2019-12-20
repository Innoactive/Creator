#if UNITY_EDITOR

using Innoactive.Hub.Training.SceneObjects;
using NUnit.Framework;
using UnityEngine;

namespace Innoactive.Hub.Unity.Tests.Training.Conditions
{
    public class ObjectInTargetTestBase : RuntimeTests
    {
        private readonly Vector3 targetPosition = new Vector3(10, 10, 10);
        protected Vector3 PositionFarFromTarget = new Vector3(-10, -10, -10);
        protected Vector3 PositionOffsetNearTarget = new Vector3(0.1f, 0.1f, 0.1f);

        protected GameObject TargetPositionObject;
        protected TrainingSceneObject TargetTrainingSceneObject;

        protected GameObject TrackedObject;
        protected TrainingSceneObject TrackedTrainingSceneObject;

        [SetUp]
        public void SetUpCreatePositionObject()
        {
            // Setup position target object
            TargetPositionObject = new GameObject("Position Object");
            TargetPositionObject.transform.position = targetPosition;
        }

        [SetUp]
        public void SetUpCreateTrackedObject()
        {
            // Setup tracked object
            TrackedObject = new GameObject("Tracked Object");
            TrackedObject.transform.position = PositionFarFromTarget;
        }

        [TearDown]
        public void TearDownDestroyGameObjects()
        {
            Object.DestroyImmediate(TargetPositionObject);
            Object.DestroyImmediate(TrackedObject);
        }
    }
}

#endif
