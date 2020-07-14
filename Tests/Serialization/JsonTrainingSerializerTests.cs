using System.Collections;
using System.Linq;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Behaviors;
using Innoactive.Creator.Core.Conditions;
using Innoactive.Creator.Core.Internationalization;
using Innoactive.Creator.Tests.Builder;
using Innoactive.Creator.Tests.Utils;
using Innoactive.Creator.Tests.Utils.Mocks;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace Innoactive.Creator.Tests.Serialization
{
    public class JsonTrainingSerializerTests : RuntimeTests
    {
        [UnityTest]
        public IEnumerator BaseTraining()
        {
            // Given base training
            ICourse training1 = new LinearTrainingBuilder("Training")
                .AddChapter(new LinearChapterBuilder("Chapter")
                    .AddStep(new BasicStepBuilder("Step")))
                .Build();

            Serializer.CourseToByteArray(training1);

            // When we serialize and deserialize it
            ICourse training2 = Serializer.CourseFromByteArray(Serializer.CourseToByteArray(training1));

            // Then it should still be base training, have the same name and the first chapter with the same name.
            Assert.AreEqual(typeof(Course), training1.GetType());
            Assert.AreEqual(training1.GetType(), training2.GetType());

            Assert.AreEqual(training1.Data.Name, "Training");
            Assert.AreEqual(training1.Data.Name, training2.Data.Name);

            Assert.AreEqual(training1.Data.FirstChapter.Data.Name, "Chapter");
            Assert.AreEqual(training1.Data.FirstChapter.Data.Name, training2.Data.FirstChapter.Data.Name);

            return null;
        }

        [UnityTest]
        public IEnumerator Chapter()
        {
            // Given we have a training with a chapter
            ICourse training1 = new LinearTrainingBuilder("Training")
                .AddChapter(new LinearChapterBuilder("Chapter")
                    .AddStep(new BasicStepBuilder("Step")))
                .Build();

            // When we serialize and deserialize it
            ICourse training2 = Serializer.CourseFromByteArray((Serializer.CourseToByteArray(training1)));

            // Then chapter's type, name, first step and next chapter should not change.
            IChapter chapter1 = training1.Data.FirstChapter;
            IChapter chapter2 = training2.Data.FirstChapter;

            Assert.AreEqual(chapter1.GetType(), chapter2.GetType());
            Assert.AreEqual(chapter1.Data.Name, chapter2.Data.Name);
            Assert.AreEqual(chapter1.Data.FirstStep.Data.Name, chapter2.Data.FirstStep.Data.Name);
            Assert.AreEqual(training1.Data.Chapters.Count, training2.Data.Chapters.Count);

            return null;
        }

        [UnityTest]
        public IEnumerator Condition()
        {
            // Given a training which has a step with a condition
            ICourse training1 = new LinearTrainingBuilder("Training")
                .AddChapter(new LinearChapterBuilder("Chapter")
                    .AddStep(new BasicStepBuilder("Step")
                        .AddCondition(new AutoCompletedCondition())))
                .Build();

            // When we serialize and deserialize it
            ICourse training2 = Serializer.CourseFromByteArray((Serializer.CourseToByteArray(training1)));

            // Then that condition's name should not change.
            ICondition condition1 = training1.Data.FirstChapter.Data.FirstStep.Data.Transitions.Data.Transitions.First()
                .Data.Conditions.First();
            ICondition condition2 = training2.Data.FirstChapter.Data.FirstStep.Data.Transitions.Data.Transitions.First()
                .Data.Conditions.First();

            Assert.AreEqual(condition1.GetType(), condition2.GetType());

            return null;
        }

        [UnityTest]
        public IEnumerator Transition()
        {
            // Given a training with more than one step
            ICourse training1 = new LinearTrainingBuilder("Training")
                .AddChapter(new LinearChapterBuilder("Chapter")
                    .AddStep(new BasicStepBuilder("FirstStep"))
                    .AddStep(new BasicStepBuilder("SecondStep")))
                .Build();

            // When we serialize and deserialize it
            byte[] serialized = Serializer.CourseToByteArray(training1);
            ICourse training2 = Serializer.CourseFromByteArray(serialized);

            // Then transition from the first step should lead to the same step as before.
            Assert.AreEqual(
                training1.Data.FirstChapter.Data.FirstStep.Data.Transitions.Data.Transitions.First().Data.TargetStep
                    .Data.Name,
                training2.Data.FirstChapter.Data.FirstStep.Data.Transitions.Data.Transitions.First().Data.TargetStep
                    .Data.Name);

            return null;
        }

        [UnityTest]
        public IEnumerator Step()
        {
            // Given we have a training with a step
            ICourse training1 = new LinearTrainingBuilder("Training")
                .AddChapter(new LinearChapterBuilder("Chapter")
                    .AddStep(new BasicStepBuilder("Step")
                        .AddCondition(new AutoCompletedCondition())))
                .Build();

            // When we serialize and deserialize it
            ICourse training2 = Serializer.CourseFromByteArray((Serializer.CourseToByteArray(training1)));

            // Then that step's name should still be the same.
            Assert.AreEqual(training1.Data.FirstChapter.Data.FirstStep.Data.Name,
                training2.Data.FirstChapter.Data.FirstStep.Data.Name);

            return null;
        }

        [UnityTest]
        public IEnumerator LocalizedString()
        {
            // Given a LocalizedString
            LocalizedString original = new LocalizedString("Test1{0}{1}", "Test2", "Test3", "Test4");

            Step step = new Step("");
            step.Data.Behaviors.Data.Behaviors.Add(new LocalizedStringBehaviorMock(original));
            ICourse course = new Course("", new Chapter("", step));

            // When we serialize and deserialize a training with it
            ICourse deserializedCourse = Serializer.CourseFromByteArray(Serializer.CourseToByteArray(course));
            LocalizedStringBehaviorMock deserializedBehavior =
                deserializedCourse.Data.FirstChapter.Data.FirstStep.Data.Behaviors.Data.Behaviors.First() as
                    LocalizedStringBehaviorMock;
            // ReSharper disable once PossibleNullReferenceException
            LocalizedString deserialized = deserializedBehavior.Data.LocalizedString;

            // Then deserialized training should have different instance of LocalizedString but with the same values.
            Assert.IsFalse(ReferenceEquals(original, deserialized));
            Assert.AreEqual(original.Key, deserialized.Key);
            Assert.AreEqual(original.DefaultText, deserialized.DefaultText);
            Assert.IsTrue(original.FormatParams.SequenceEqual(deserialized.FormatParams));

            yield return null;
        }
    }
}
