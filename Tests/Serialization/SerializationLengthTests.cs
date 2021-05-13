using System;
using System.Linq;
using VPG.Creator.Core;
using VPG.Creator.Core.Serialization;
using VPG.Creator.Tests.Builder;
using NUnit.Framework;

namespace VPG.CreatorEditor.Tests
{
    public class SerializationLengthTests
    {

        [Test]
        public void RunSerializerWithLinear()
        {
            int length = 100;
            ICourse course = CreateTrainingCourse(length);

            ImprovedNewtonsoftJsonCourseSerializer serializer = new ImprovedNewtonsoftJsonCourseSerializer();
            try
            {
                byte[] data = serializer.CourseToByteArray(course);
                serializer.CourseFromByteArray(data);
            }
            catch (Exception)
            {
                Assert.Fail("failed with a length of: " + length);
            }
        }

        [Test]
        public void RunSerializerWithSplit()
        {
            int length = 200;
            ICourse course = CreateSplitTrainingCourse(length);

            ImprovedNewtonsoftJsonCourseSerializer serializer = new ImprovedNewtonsoftJsonCourseSerializer();
            try
            {
                byte[] data = serializer.CourseToByteArray(course);
                serializer.CourseFromByteArray(data);
            }
            catch (Exception)
            {
                Assert.Fail("failed with a length of: " + length);
            }
        }

        [Test]
        public void RunSerializerWithEarlyFinish()
        {
            int length = 500;
            ICourse course = CreateTrainingCourse(length);

            ImprovedNewtonsoftJsonCourseSerializer serializer = new ImprovedNewtonsoftJsonCourseSerializer();
            try
            {
                Transition t1 = new Transition();
                t1.Data.TargetStep = null;
                course.Data.Chapters[0].Data.Steps.First().Data.Transitions.Data.Transitions.Insert(0, t1);

                byte[] data = serializer.CourseToByteArray(course);
                serializer.CourseFromByteArray(data);
            }
            catch (Exception)
            {
                Assert.Fail("failed with a length of: " + length);
            }
        }

        private ICourse CreateTrainingCourse(int length)
        {

            LinearChapterBuilder chapterBuilder = new LinearChapterBuilder("chapter");
            for (int i = 0; i < length; i++)
            {
                chapterBuilder.AddStep(new BasicStepBuilder("Step#" + i));
            }

            return new LinearTrainingBuilder("Training")
                .AddChapter(chapterBuilder)
                .Build();
        }

        private ICourse CreateSplitTrainingCourse(int length)
        {

            LinearChapterBuilder[] chapterBuilder = new[] {new LinearChapterBuilder("chapter"), new LinearChapterBuilder("chapter"), new LinearChapterBuilder("chapter")};

            for (int c = 0; c < 3; c++)
            {
                for (int i = 0; i < length; i++)
                {
                    chapterBuilder[c].AddStep(new BasicStepBuilder("Step#" + i));
                }
            }

            Chapter chapter = chapterBuilder[0].Build();


            Chapter c1 = chapterBuilder[1].Build();
            Transition t1 = new Transition();
            t1.Data.TargetStep = c1.Data.FirstStep;


            Chapter c2 = chapterBuilder[1].Build();
            Transition t2 = new Transition();
            t2.Data.TargetStep = c2.Data.FirstStep;


            chapter.Data.FirstStep.Data.Transitions.Data.Transitions.Add(t1);
            chapter.Data.FirstStep.Data.Transitions.Data.Transitions.Add(t2);


            Transition t1end = new Transition();
            t1end.Data.TargetStep = chapter.Data.Steps.Last();
            c1.Data.Steps.Last().Data.Transitions.Data.Transitions.Add(t1end);


            Transition t2end = new Transition();
            t2end.Data.TargetStep = chapter.Data.Steps.Last();
            c2.Data.Steps.Last().Data.Transitions.Data.Transitions.Add(t2end);

            c1.Data.Steps.ToList().ForEach(chapter.Data.Steps.Add);
            c2.Data.Steps.ToList().ForEach(chapter.Data.Steps.Add);

            return new Course("name", chapter);
        }
    }
}
