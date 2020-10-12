using Innoactive.Creator.Core.Audio;
using Innoactive.Creator.Core.Internationalization;
using Innoactive.Creator.Core.SceneObjects;
using Innoactive.Creator.Core.Utils;
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
            Assert.IsFalse(ReflectionUtils.IsEmpty(false), "boolean cannot be empty");
            Assert.IsFalse(ReflectionUtils.IsEmpty(true), "boolean cannot be empty");
        }

        [Test]
        public void Float()
        {
            Assert.IsFalse(ReflectionUtils.IsEmpty(-1), "negative numbers should work");
            Assert.IsFalse(ReflectionUtils.IsEmpty(1), "positive numbers should work");
            Assert.IsFalse(ReflectionUtils.IsEmpty(0), "default value of the number should trigger an error");
        }

        [Test]
        public void String()
        {
            Assert.IsFalse(ReflectionUtils.IsEmpty("text"));
            Assert.IsTrue(ReflectionUtils.IsEmpty(null), "Null should trigger an error");
            Assert.IsTrue(ReflectionUtils.IsEmpty(""), "Empty text should trigger an error");
        }

        [Test]
        public void ResourceAudioData()
        {
            Localization.entries.Add("key", "key");
            ResourceAudio source = new ResourceAudio(new LocalizedString());
            Assert.IsTrue(ReflectionUtils.IsEmpty(new ResourceAudio(new LocalizedString())), "Empty audio resources should throw an error");
            Assert.IsFalse(ReflectionUtils.IsEmpty(new ResourceAudio(new LocalizedString("key", ""))));
            Assert.IsFalse(ReflectionUtils.IsEmpty(new ResourceAudio(new LocalizedString("", "text"))));
            Assert.IsFalse(ReflectionUtils.IsEmpty(new ResourceAudio(new LocalizedString("key", "text"))));
        }

        [Test]
        public void SceneObjectReference()
        {
            TestingUtils.CreateSceneObject("test");
            Assert.IsFalse(ReflectionUtils.IsEmpty(new SceneObjectReference("test")));
            Assert.IsTrue(ReflectionUtils.IsEmpty(new SceneObjectReference()), "Not set reference should be an error");
            Assert.IsTrue(ReflectionUtils.IsEmpty(new SceneObjectReference("test2")), "Not existing reference should be an error");
        }

        [Test]
        public void ScenePropertyReference()
        {
            TrainingSceneObject obj = TestingUtils.CreateSceneObject("test");
            obj.gameObject.AddComponent<PropertyMock>();

            Assert.IsFalse(ReflectionUtils.IsEmpty(new ScenePropertyReference<PropertyMock>("test")));
            Assert.IsTrue(ReflectionUtils.IsEmpty(new ScenePropertyReference<PropertyMock>()), "Not set reference should be an error");
            Assert.IsTrue(ReflectionUtils.IsEmpty(new ScenePropertyReference<PropertyMock>("test2")), "Not existing reference should be an error");
        }
    }
}
