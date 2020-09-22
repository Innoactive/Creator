using Innoactive.Creator.Core;
using Innoactive.Creator.Core.SceneObjects;
using Innoactive.Creator.Tests.Utils;
using Innoactive.CreatorEditor.CourseValidation;
using NUnit.Framework;

namespace Innoactive.CreatorEditor.Tests.CourseValidation
{
    internal class StepConnectionValidatorTests : RuntimeTests
    {
        [Test]
        public void MultipleLinkedStepsWork()
        {
            TrainingSceneObject obj = TestingUtils.CreateSceneObject("test");
            TestLinearChapterBuilder builder = TestLinearChapterBuilder.SetupChapterBuilder();
            builder.AddStep("s1");
            builder.AddStep("s2");
            builder.AddStep("s3");

            IChapter chapter = builder.Build();

            Assert.AreEqual(0, new StepConnectionValidator().Validate(chapter, new ChapterContext(chapter, null)).Count);
        }

        [Test]
        public void OneStepWorks()
        {
            TrainingSceneObject obj = TestingUtils.CreateSceneObject("test");
            TestLinearChapterBuilder builder = TestLinearChapterBuilder.SetupChapterBuilder();
            builder.AddStep("s1");

            IChapter chapter = builder.Build();

            Assert.AreEqual(0, new StepConnectionValidator().Validate(chapter, new ChapterContext(chapter, null)).Count);
        }

        [Test]
        public void NoStepWorks()
        {
            TrainingSceneObject obj = TestingUtils.CreateSceneObject("test");
            TestLinearChapterBuilder builder = TestLinearChapterBuilder.SetupChapterBuilder();
            IChapter chapter = builder.Build();

            Assert.AreEqual(0, new StepConnectionValidator().Validate(chapter, new ChapterContext(chapter, null)).Count);
        }

        [Test]
        public void NotLinkedStepsFail()
        {
            TrainingSceneObject obj = TestingUtils.CreateSceneObject("test");
            TestLinearChapterBuilder builder = TestLinearChapterBuilder.SetupChapterBuilder();
            builder.AddStep("s1");
            builder.AddStep("s2");
            builder.AddStep("s3");
            IChapter chapter = builder.Build();

            chapter.Data.Steps[0].Data.Transitions.Data.Transitions[0].Data.TargetStep = null;


            Assert.AreEqual(1, new StepConnectionValidator().Validate(chapter, new ChapterContext(chapter, null)).Count);
        }
    }
}
