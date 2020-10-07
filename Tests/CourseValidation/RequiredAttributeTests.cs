using Innoactive.Creator.Core.Audio;
using Innoactive.Creator.Core.Internationalization;
using Innoactive.Creator.Core.SceneObjects;
using Innoactive.Creator.Core.Validation;
using Innoactive.Creator.Tests.Utils;
using Innoactive.Creator.Tests.Utils.Mocks;
using NUnit.Framework;

namespace Innoactive.CreatorEditor.Tests.CourseValidation
{
    internal class RequiredAttributeTests : RuntimeTests
    {
        [Test]
        public void Boolean()
        {
            Assert.AreEqual(0, new RequiredAttribute().Validate((object) false).Count, "boolean values should never trigger an error");
            Assert.AreEqual(0, new RequiredAttribute().Validate((object) true).Count, "boolean values should never trigger an error");
        }

        [Test]
        public void Float()
        {
            Assert.AreEqual(0, new RequiredAttribute().Validate((object) -1).Count, "negative numbers should work");
            Assert.AreEqual(0, new RequiredAttribute().Validate((object) 1).Count, "positive numbers should work");
            Assert.AreEqual(1, new RequiredAttribute().Validate((object) 0).Count, "default value of the number should trigger an error");
        }

        [Test]
        public void String()
        {
            Assert.AreEqual(0, new RequiredAttribute().Validate((object) "text").Count);
            Assert.AreEqual(1, new RequiredAttribute().Validate((object) null).Count, "Null should trigger an error");
            Assert.AreEqual(1, new RequiredAttribute().Validate((object) "").Count, "Empty text should trigger an error");
        }

        [Test]
        public void ResourceAudioData()
        {
            Localization.entries.Add("key", "key");
            ResourceAudio source = new ResourceAudio(new LocalizedString());
            Assert.AreEqual(1, new RequiredAttribute().Validate((object) new ResourceAudio(new LocalizedString())).Count, "Empty audio resources should throw an error");
            Assert.AreEqual(0, new RequiredAttribute().Validate((object) new ResourceAudio(new LocalizedString("key", ""))).Count);
            Assert.AreEqual(0, new RequiredAttribute().Validate((object) new ResourceAudio(new LocalizedString("", "text"))).Count);
            Assert.AreEqual(0, new RequiredAttribute().Validate((object) new ResourceAudio(new LocalizedString("key", "text"))).Count);
        }

        [Test]
        public void SceneObjectReference()
        {
            TestingUtils.CreateSceneObject("test");
            Assert.AreEqual(0, new RequiredAttribute().Validate((object) new SceneObjectReference("test")).Count);
            Assert.AreEqual(1, new RequiredAttribute().Validate((object) new SceneObjectReference()).Count, "Not set reference should be an error");
            Assert.AreEqual(1, new RequiredAttribute().Validate((object) new SceneObjectReference("test2")).Count, "Not existing reference should be an error");
        }

        [Test]
        public void ScenePropertyReference()
        {
            TrainingSceneObject obj = TestingUtils.CreateSceneObject("test");
            obj.gameObject.AddComponent<PropertyMock>();

            Assert.AreEqual(0, new RequiredAttribute().Validate((object) new ScenePropertyReference<PropertyMock>("test")).Count);
            Assert.AreEqual(1, new RequiredAttribute().Validate((object) new ScenePropertyReference<PropertyMock>()).Count, "Not set reference should be an error");
            Assert.AreEqual(1, new RequiredAttribute().Validate((object) new ScenePropertyReference<PropertyMock>("test2")).Count, "Not existing reference should be an error");
        }
    }
}
