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
    }
}
