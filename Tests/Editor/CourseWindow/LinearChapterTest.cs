using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using VPG.Creator.Core;
using VPG.CreatorEditor.UI.Windows;

namespace VPG.CreatorEditor.Tests.CourseWindowTests
{
    internal class LinearChapterTest : BaseCourseWindowTest
    {
        private int iteratedSteps;
        private List<IStep> validatedSteps = new List<IStep>();

        /// <inheritdoc />
        public override string WhenDescription
        {
            get
            {
                return "Add a step to the chapter. Connect it to the chapter's start node.\n" +
                    "Add a second chapter.\n" +
                    "Add a step to that chapter and connect it to the chapter's start node.";
            }
        }

        /// <inheritdoc />
        public override string ThenDescription
        {
            get
            {
                return "There is a training with exactly one execution flow.";
            }
        }

        /// <inheritdoc />
        protected override CourseWindow Given()
        {
            CourseWindow window = base.Given();
            iteratedSteps = 0;

            return window;
        }

        /// <inheritdoc />
        protected override void Then(CourseWindow window)
        {
            ICourse result = ExtractTraining(window);
            IChapter firstChapter = result.Data.Chapters.First();

            Assert.NotNull(firstChapter);

            IStep firstStep = firstChapter.Data.FirstStep;
            IList<IStep> steps = firstChapter.Data.Steps;

            validatedSteps.Clear();
            ValidateLinearCourse(firstStep);

            Assert.That(iteratedSteps == steps.Count);
        }

        private void ValidateLinearCourse(IStep step)
        {
            iteratedSteps++;

            // The step exits
            Assert.NotNull(step);

            // The step was not validated before (In case of circular flows).
            Assert.IsFalse(validatedSteps.Contains(step));

            IList<ITransition> transitions = GetTransitionsFromStep(step);

            // It has only one transition.
            Assert.That(transitions.Count == 1);
            validatedSteps.Add(step);
            IStep nextStep;

            // In case the transition points to another step, the process starts again.
            // If not, the step is the end of the chapter.
            if (TryToGetStepFromTransition(transitions.First(), out nextStep))
            {
                ValidateLinearCourse(nextStep);
            }
        }
    }
}
