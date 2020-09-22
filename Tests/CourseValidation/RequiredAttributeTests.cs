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
            string message;
            Assert.False(new RequiredAttribute().Validate((object) false, out message), "boolean values should never trigger an error");
            Assert.False(new RequiredAttribute().Validate((object) true, out message), "boolean values should never trigger an error");
        }

        [Test]
        public void Float()
        {
            string message;
            Assert.False(new RequiredAttribute().Validate((object) -1, out message), "negative numbers shuold work");
            Assert.False(new RequiredAttribute().Validate((object) 1, out message), "positive numbers should work");
            Assert.True(new RequiredAttribute().Validate((object) 0, out message), "default value of the number should trigger an error");
        }

        [Test]
        public void String()
        {
            string message;
            Assert.False(new RequiredAttribute().Validate((object) "text", out message));
            Assert.True(new RequiredAttribute().Validate((object) null, out message), "Null should trigger an error");
            Assert.True(new RequiredAttribute().Validate((object) "", out message), "Empty text should trigger an error");
        }

        [Test]
        public void ResourceAudioData()
        {
            string message;

            Localization.entries.Add("key", "key");
            ResourceAudio source = new ResourceAudio(new LocalizedString());
            Assert.True(new RequiredAttribute().Validate((object) new ResourceAudio(new LocalizedString()), out message), "Empty audio resources should throw an error");
            Assert.False(new RequiredAttribute().Validate((object) new ResourceAudio(new LocalizedString("key", "")), out message));
            Assert.False(new RequiredAttribute().Validate((object) new ResourceAudio(new LocalizedString("", "text")), out message));
            Assert.False(new RequiredAttribute().Validate((object) new ResourceAudio(new LocalizedString("key", "text")), out message));
        }

        [Test]
        public void SceneObjectReference()
        {
            string message;

            TestingUtils.CreateSceneObject("test");
            Assert.False(new RequiredAttribute().Validate((object) new SceneObjectReference("test"), out message));
            Assert.True(new RequiredAttribute().Validate((object) new SceneObjectReference(), out message), "Not set reference should be an error");
            Assert.True(new RequiredAttribute().Validate((object) new SceneObjectReference("test2"), out message), "Not existing reference should be an error");
        }

        [Test]
        public void ScenePropertyReference()
        {
            string message;

            TrainingSceneObject obj = TestingUtils.CreateSceneObject("test");
            obj.gameObject.AddComponent<PropertyMock>();

            Assert.False(new RequiredAttribute().Validate((object) new ScenePropertyReference<PropertyMock>("test"), out message));
            Assert.True(new RequiredAttribute().Validate((object) new ScenePropertyReference<PropertyMock>(), out message), "Not set reference should be an error");
            Assert.True(new RequiredAttribute().Validate((object) new ScenePropertyReference<PropertyMock>("test2"), out message), "Not existing reference should be an error");
        }
    }
}
