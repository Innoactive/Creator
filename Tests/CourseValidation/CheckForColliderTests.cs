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
            TrainingSceneObject obj = TestingUtils.CreateSceneObject("test");
            Assert.AreEqual(1, new CheckForColliderAttribute().Validate((object) new SceneObjectReference("test")).Count);
        }

        [Test]
        public void NoErrorWhenColliderExists()
        {
            TrainingSceneObject obj = TestingUtils.CreateSceneObject("test");
            obj.gameObject.AddComponent<BoxCollider>();

            Assert.AreEqual(0, new CheckForColliderAttribute().Validate((object) new SceneObjectReference("test")).Count);
        }
    }
}
