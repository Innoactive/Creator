using System;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Behaviors;
using Innoactive.Creator.Core.Conditions;
using Innoactive.Creator.Core.Internationalization;
using Innoactive.Creator.Core.SceneObjects;
using Innoactive.Creator.Core.Properties;
using Innoactive.Creator.Core.Utils.Builders;
using Innoactive.Creator.Tests.Utils;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Innoactive.Creator.Tests.Builder
{
    public class TrainingBuilderTests : RuntimeTests
    {
        [Test]
        public void SimplestTrainingBuilderTest()
        {
            // Given a builder of a training with one chapter with one step
            LinearTrainingBuilder builder = new LinearTrainingBuilder("Training1")
                .AddChapter(new LinearChapterBuilder("Chapter1.1")
                    .AddStep(new BasicStepBuilder("Step1.1.1"))
                );

            // When we build a training from it
            ICourse course = builder.Build();

            // Then it consists of exactly one chapter and one step, and their names are the same as expected
            Assert.True(course.Data.Name == "Training1");
            Assert.True(course.Data.FirstChapter.Data.Name == "Chapter1.1");
            Assert.True(course.Data.FirstChapter.Data.FirstStep.Data.Name == "Step1.1.1");
            Assert.True(course.Data.FirstChapter.Data.FirstStep.Data.Transitions.Data.Transitions.Count == 1);
            Assert.True(course.Data.FirstChapter.Data.FirstStep.Data.Transitions.Data.Transitions.First().Data.TargetStep == null);
            Assert.AreEqual(1, course.Data.Chapters.Count);
        }

        [Test]
        public void OneChapterMultipleStepsTest()
        {
            // Given a builder of a training with one chapter with three steps
            LinearTrainingBuilder builder = new LinearTrainingBuilder("Training1")
                .AddChapter(new LinearChapterBuilder("Chapter1.1")
                    .AddStep(new BasicStepBuilder("Step1.1.1"))
                    .AddStep(new BasicStepBuilder("Step1.1.2"))
                    .AddStep(new BasicStepBuilder("Step1.1.3")));

            // When we build a training from it
            ICourse course = builder.Build();

            // Then it has exactly three steps in the same order.
            IStep firstStep = course.Data.FirstChapter.Data.FirstStep;
            Assert.True(firstStep.Data.Name == "Step1.1.1");
            IStep secondStep = firstStep.Data.Transitions.Data.Transitions.First().Data.TargetStep;
            Assert.True(secondStep.Data.Name == "Step1.1.2");
            IStep thirdStep = secondStep.Data.Transitions.Data.Transitions.First().Data.TargetStep;
            Assert.True(thirdStep.Data.Name == "Step1.1.3");
            Assert.True(thirdStep.Data.Transitions.Data.Transitions.First().Data.TargetStep == null);
        }

        [Test]
        public void MultipleChaptersTest()
        {
            // Given a builder of a training with three chapters with one, three, and one steps
            LinearTrainingBuilder builder = new LinearTrainingBuilder("1")
                .AddChapter(new LinearChapterBuilder("1.1")
                    .AddStep(new BasicStepBuilder("1.1.1")))
                .AddChapter(new LinearChapterBuilder("1.2")
                    .AddStep(new BasicStepBuilder("1.2.1"))
                    .AddStep(new BasicStepBuilder("1.2.2"))
                    .AddStep(new BasicStepBuilder("1.2.3")))
                .AddChapter(new LinearChapterBuilder("1.3")
                    .AddStep(new BasicStepBuilder("1.3.1")));

            // When we build a training from it
            ICourse course = builder.Build();

            // Then it has exactly three chapters in it with one, three, and one steps,
            // `NextChapter` properties are properly assigned,
            // and every chapter has expected composition of steps.
            IChapter chapter = course.Data.FirstChapter;
            Assert.True(chapter.Data.Name == "1.1");
            IStep step = chapter.Data.FirstStep;
            Assert.True(chapter.Data.FirstStep.Data.Name == "1.1.1");
            Assert.True(step.Data.Transitions.Data.Transitions.First().Data.TargetStep == null);

            chapter = course.Data.Chapters[1];
            Assert.True(chapter.Data.Name == "1.2");
            step = chapter.Data.FirstStep;
            Assert.True(step.Data.Name == "1.2.1");
            step = step.Data.Transitions.Data.Transitions.First().Data.TargetStep;
            Assert.True(step.Data.Name == "1.2.2");
            step = step.Data.Transitions.Data.Transitions.First().Data.TargetStep;
            Assert.True(step.Data.Name == "1.2.3");
            Assert.True(step.Data.Transitions.Data.Transitions.First().Data.TargetStep == null);

            chapter = course.Data.Chapters[2];
            Assert.True(chapter.Data.Name == "1.3");
            step = chapter.Data.FirstStep;
            Assert.True(step.Data.Name == "1.3.1");
            Assert.True(step.Data.Transitions.Data.Transitions.First().Data.TargetStep == null);
            Assert.AreEqual(3, course.Data.Chapters.Count);
        }

        [Test]
        public void ReuseBuilderTest()
        {
            // Given a builder
            LinearTrainingBuilder builder = new LinearTrainingBuilder("1")
                .AddChapter(new LinearChapterBuilder("1.1")
                    .AddStep(new BasicStepBuilder("1.1.1")))
                .AddChapter(new LinearChapterBuilder("1.2")
                    .AddStep(new BasicStepBuilder("1.2.1"))
                    .AddStep(new BasicStepBuilder("1.2.2"))
                    .AddStep(new BasicStepBuilder("1.2.3")))
                .AddChapter(new LinearChapterBuilder("1.3")
                    .AddStep(new BasicStepBuilder("1.3.1")));

            // When we build two trainings from it
            ICourse training1 = builder.Build();
            ICourse training2 = builder.Build();

            Assert.True(training1.Data.Chapters.Count == training2.Data.Chapters.Count, "Both trainings should have the same length");

            // Then two different instances of the training are created,
            // which have the same composition of chapters and steps,
            // but there is not a single step or chapter instance that is shared between two trainings.
            for (int i = 0; i < 3; i++)
            {
                IChapter chapter1 = training1.Data.Chapters[i];
                IChapter chapter2 = training2.Data.Chapters[i];

                Assert.False(ReferenceEquals(chapter1, chapter2));
                Assert.True(chapter1.Data.Name == chapter2.Data.Name);

                IStep step1 = chapter1.Data.FirstStep;
                IStep step2 = chapter2.Data.FirstStep;

                while (step1 != null)
                {
                    Assert.False(ReferenceEquals(step1, step2));
                    Assert.True(step1.Data.Name == step2.Data.Name);

                    step1 = step1.Data.Transitions.Data.Transitions.First().Data.TargetStep;
                    step2 = step2.Data.Transitions.Data.Transitions.First().Data.TargetStep;
                }

                Assert.True(step2 == null, "If we are here, step1 is null. If step1 is null, step2 has to be null, too.");
            }
        }

        [Test]
        public void BuildingIntroTest()
        {
            // Given a builder with a predefined Intro step
            LinearTrainingBuilder builder = new LinearTrainingBuilder("TestTraining")
                .AddChapter(new LinearChapterBuilder("TestChapter")
                    .AddStep(DefaultSteps.Intro("TestIntroStep")));

            // When we build a training from it,
            IStep step = builder.Build().Data.FirstChapter.Data.FirstStep;

            // Then a training with an Intro step is created.
            Assert.True(step != null);
            Assert.True(step.Data.Name == "TestIntroStep");
            Assert.True(step.Data.Transitions.Data.Transitions.First().Data.Conditions.Any() == false);
        }

        [UnityTest]
        public IEnumerator BuildingColliderPutTest()
        {
            // Given two training scene objects with `ColliderWithTriggerProperty` and a builder for at raining with a PutIntoCollider default step
            GameObject colliderGo = new GameObject("Collider");
            TrainingSceneObject testCollider = colliderGo.AddComponent<TrainingSceneObject>();
            colliderGo.AddComponent<SphereCollider>().isTrigger = true;
            colliderGo.AddComponent<ColliderWithTriggerProperty>();
            testCollider.ChangeUniqueName("Collider");

            GameObject putGo = new GameObject("Puttable");
            TrainingSceneObject testObjectToPut = putGo.AddComponent<TrainingSceneObject>();
            putGo.AddComponent<SphereCollider>().isTrigger = true;
            putGo.AddComponent<ColliderWithTriggerProperty>();
            testObjectToPut.ChangeUniqueName("ToPut");

            LinearTrainingBuilder builder = new LinearTrainingBuilder("TestTraining")
                .AddChapter(new LinearChapterBuilder("TestChapter")
                    .AddStep(DefaultSteps.PutIntoCollider("TestColliderPutStep", "Collider", 1f, "ToPut")));

            // When you build a training from it
            IStep step = builder.Build().Data.FirstChapter.Data.FirstStep;

            // Then it has a step with a ObjectInColliderCondition.
            Assert.True(step != null);
            Assert.True(step.Data.Name == "TestColliderPutStep");
            Assert.True(step.Data.Transitions.Data.Transitions.First().Data.Conditions.Count == 1);
            Assert.True(step.Data.Transitions.Data.Transitions.First().Data.Conditions.First() is ObjectInColliderCondition);

            // Cleanup
            Object.DestroyImmediate(colliderGo);
            Object.DestroyImmediate(putGo);

            return null;
        }

        [UnityTest]
        public IEnumerator HighlightTest()
        {
            // Given we have a training scene object and a builder for a training with a step with highlight that object
            GameObject go = new GameObject("Highlightable");
            EnableHighlightProperty highlightable = go.AddComponent<EnableHighlightProperty>();
            highlightable.SceneObject.ChangeUniqueName("Highlightable");

            LinearTrainingBuilder builder = new LinearTrainingBuilder("TestTraining")
                .AddChapter(new LinearChapterBuilder("TestChapter")
                    .AddStep(new BasicStepBuilder("TestHighlightStep")
                        .Highlight("Highlightable")));

            // When we build a training from it
            IStep step = builder.Build().Data.FirstChapter.Data.FirstStep;

            // Then we have a step with VRTKObjectHighlight behavior.
            Assert.True(step != null);
            Assert.True(step.Data.Name == "TestHighlightStep");
            Assert.True(step.Data.Behaviors.Data.Behaviors.First() is HighlightObjectBehavior);
            Assert.True(ReferenceEquals((step.Data.Behaviors.Data.Behaviors.First() as HighlightObjectBehavior).Data.ObjectToHighlight.Value, highlightable));

            // Cleanup
            Object.DestroyImmediate(go);

            return null;
        }

        [UnityTest]
        public IEnumerator CallAddAudioDescriptionTwiceTest()
        {
            TestDelegate test = () =>
            {
                // Given a builder with two .AddAudioDescription calls
                LinearTrainingBuilder builder = new LinearTrainingBuilder("TestTraining")
                    .AddChapter(new LinearChapterBuilder("TestChapter")
                        .AddStep(new BasicStepBuilder("TestStep")
                            .AddAudioDescription(new LocalizedString("Path1"))
                            .AddAudioDescription(new LocalizedString("Path2"))));

                // When we build a training from it
                builder.Build();
            };

            // Then an exception should be thrown out, as having two audio descriptions is not allowed.
            Assert.Throws<InvalidOperationException>(test);

            return null;
        }

        [UnityTest]
        public IEnumerator CallAddAudioSuccessTwiceTest()
        {
            TestDelegate test = () =>
            {
                // Given a builder with two .AddAudioSuccess calls
                LinearTrainingBuilder builder = new LinearTrainingBuilder("TestTraining")
                    .AddChapter(new LinearChapterBuilder("TestChapter")
                        .AddStep(new BasicStepBuilder("TestStep")
                            .AddAudioSuccess(new LocalizedString("Path1"))
                            .AddAudioSuccess(new LocalizedString("Path2"))));

                // When we build a training from it
                builder.Build();
            };

            // Then an exception should be thrown out, as having two audios for step completion is not allowed.
            Assert.Throws<InvalidOperationException>(test);

            return null;
        }

        [UnityTest]
        public IEnumerator CallAddAudioHintTwiceTest()
        {
            TestDelegate test = () =>
            {
                // Given a builder with two .AddAudioHint calls
                LinearTrainingBuilder builder = new LinearTrainingBuilder("TestTraining")
                    .AddChapter(new LinearChapterBuilder("TestChapter")
                        .AddStep(new BasicStepBuilder("TestStep")
                            .AddAudioHint(new LocalizedString("Path1"))
                            .AddAudioHint(new LocalizedString("Path2"))));

                // When we build a training from it
                builder.Build();
            };

            //Then an exception should be thrown out, as having two audio hints is not allowed.
            Assert.Throws<InvalidOperationException>(test);

            return null;
        }

        /// <summary>
        /// </summary>
        [UnityTest]
        public IEnumerator AudioFlagsCleanupTest()
        {
            // Given a builder for a training
            LinearTrainingBuilder builder = new LinearTrainingBuilder("TestTraining")
                .AddChapter(new LinearChapterBuilder("TestChapter")
                    .AddStep(new BasicStepBuilder("TestStep")
                        .AddAudioSuccess(new LocalizedString("Path1"))
                        .AddAudioDescription(new LocalizedString("Path1"))
                        .AddAudioHint(new LocalizedString("Path2"))));

            // When we are building two trainings from it
            builder.Build();
            builder.Build();

            // Then it throws no exceptions. If internal builder information wasn't reset properly, then InvalidOperationException will be thrown. Not having any asserts is intended.
            return null;
        }
    }
}
