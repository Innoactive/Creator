using Innoactive.Creator.Core.SceneObjects;
using Innoactive.Creator.Core.Validation;
using Innoactive.Creator.Tests.Utils;
using NUnit.Framework;
using UnityEngine;

namespace Innoactive.CreatorEditor.Tests.CourseValidation
{
    internal class CheckForColliderTests : RuntimeTests
    {
        [Test]
        public void MissingColliderTriggersError()
        {
            string message;

            TrainingSceneObject obj = TestingUtils.CreateSceneObject("test");
            Assert.True(new CheckForColliderAttribute().Validate((object) new SceneObjectReference("test"), out message));
        }

        [Test]
        public void NoErrorWhenColliderExists()
        {
            string message;

            TrainingSceneObject obj = TestingUtils.CreateSceneObject("test");
            obj.gameObject.AddComponent<BoxCollider>();

            Assert.False(new CheckForColliderAttribute().Validate((object) new SceneObjectReference("test"), out message));
        }
    }
}
