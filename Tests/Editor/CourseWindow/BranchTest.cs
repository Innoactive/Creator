﻿using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using VPG.Creator.Core;
using VPG.CreatorEditor.UI.Windows;

namespace VPG.CreatorEditor.Tests.CourseWindowTests
{
    internal class BranchTest : BaseCourseWindowTest
    {
        /// <inheritdoc />
        public override string WhenDescription
        {
            get
            {
                return "1. Create three steps.\n" +
                    "2. Connect one of them to the beginning of the chapter.\n" +
                    "3. Add a second transition to that step.\n" +
                    "4. Connect first step to the other two steps.\n";
            }
        }

        /// <inheritdoc />
        public override string ThenDescription
        {
            get
            {
                return "There is a 2-way branched flow.";
            }
        }

        /// <inheritdoc />
        protected override void Then(CourseWindow window)
        {
            ICourse result = ExtractTraining(window);

            IChapter firstChapter = result.Data.Chapters.First();
            Assert.NotNull(firstChapter);

            IStep firstStep = firstChapter.Data.FirstStep;
            Assert.NotNull(firstStep);

            IList<ITransition> transitions = GetTransitionsFromStep(firstStep);
            Assert.That(transitions.Count == 2);

            foreach (ITransition transition in transitions)
            {
                IStep nextStep;

                if (TryToGetStepFromTransition(transition, out nextStep) == false)
                {
                    Assert.Fail("First step does not always go to another step.");
                }

                IList<ITransition> transitionsFromNextStep = GetTransitionsFromStep(nextStep);
                Assert.That(transitionsFromNextStep.Count == 1);

                IStep endOfChapter;

                if (TryToGetStepFromTransition(transitionsFromNextStep.First(), out endOfChapter))
                {
                    Assert.Fail("Branched step is not the end of the chapter.");
                }
            }
        }
    }
}
