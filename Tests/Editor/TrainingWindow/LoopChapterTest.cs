using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Innoactive.Creator.Core;
using Innoactive.CreatorEditor.UI.Windows;

namespace Innoactive.CreatorEditor.Tests.CourseWindowTests
{
    internal class LoopChapterTest : BaseTest
    {
        /// <inheritdoc />
        public override string WhenDescription
        {
            get
            {
                return "User adds a step with two transitions. The first transition points to the same step, the second one is the end of the chapter.";
            }
        }

        /// <inheritdoc />
        public override string ThenDescription
        {
            get
            {
                return "There is a basic training with a loop execution flow.";
            }
        }

        /// <inheritdoc />
        protected override CourseWindow Given()
        {
            CourseWindow window = base.Given();

            return window;
        }

        /// <inheritdoc />
        protected override void Then(CourseWindow window)
        {
            ICourse result = ExtractTraining(window);
            IChapter firstChapter = result.Data.Chapters.First();

            // The chapter exits
            Assert.NotNull(firstChapter);

            // The step exits
            IStep firstStep = firstChapter.Data.FirstStep;
            Assert.NotNull(firstChapter);

            IList<ITransition> transitions = GetTransitionsFromStep(firstStep);

            // It has two transition.
            Assert.That(transitions.Count == 2);

            ITransition firstTransition = transitions[0];
            ITransition secondTransition = transitions[1];
            IStep nextStep;

            // The first step's transition points to itself.
            if (TryToGetStepFromTransition(firstTransition, out nextStep))
            {
                Assert.That(firstStep == nextStep);
            }

            // The second step's transition is the end of the course.
            Assert.False(TryToGetStepFromTransition(secondTransition, out nextStep));
        }
    }
}
