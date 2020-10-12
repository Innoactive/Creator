using Innoactive.CreatorEditor.CourseValidation;
using NUnit.Framework;

namespace Innoactive.Creator.Core.Tests.Editor.CourseValidation
{
    public class ContextTests
    {
        [Test]
        public void SameContextIsEquals()
        {
            Course course = new Course("", new Chapter("", new Step("")));
            IContext context = new CourseContext(course.Data);

            Assert.AreEqual(context, context);
        }

        [Test]
        public void DifferentContextAreNotEqual()
        {
            Course course = new Course("", new Chapter("", new Step("")));
            Course course2 = new Course("", new Chapter("", new Step("")));
            IContext context = new CourseContext(course.Data);
            IContext context2 = new CourseContext(course2.Data);

            Assert.AreNotEqual(context, context2);
        }

        [Test]
        public void DifferentContextStillEqualWithSameEntity()
        {
            Course course = new Course("", new Chapter("", new Step("")));
            IContext context = new CourseContext(course.Data);
            IContext context2 = new CourseContext(course.Data);

            Assert.AreEqual(context, context2);
        }

        [Test]
        public void DetectIsChildCorrectly()
        {
            Course course = new Course("", new Chapter("", new Step("")));
            CourseContext courseContext = new CourseContext(course.Data);
            ChapterContext chapterContext = new ChapterContext(course.Data.Chapters[0].Data, courseContext);
            StepContext stepContext = new StepContext(chapterContext.EntityData.FirstStep.Data, chapterContext);

            Assert.IsTrue(chapterContext.IsChildOf(courseContext));
            Assert.IsTrue(stepContext.IsChildOf(chapterContext));
            Assert.IsTrue(stepContext.IsChildOf(courseContext));
        }
    }
}
