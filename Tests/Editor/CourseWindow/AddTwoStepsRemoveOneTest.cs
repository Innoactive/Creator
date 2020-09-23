using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using Innoactive.Creator.Core;
using Innoactive.CreatorEditor.UI.Windows;

namespace Innoactive.CreatorEditor.Tests.CourseWindowTests
{
    internal class AddTwoStepsRemoveOneTest : BaseCourseWindowTest
    {
        /// <inheritdoc />
        public override string WhenDescription
        {
            get
            {
                return "1. Add two new steps." + "\n" +
                       "2. Delete one step." + "\n" +
                       "3. Connect the step to the start of the chapter.";
            }
        }

        /// <inheritdoc />
        public override string ThenDescription
        {
            get
            {
                return "There is a training with exactly one step created.";
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
            Assert.That(transitions.Count == 1);

            IStep nextStep;

            if (TryToGetStepFromTransition(transitions.First(), out nextStep))
            {
                Assert.Fail("First step is not the end of the chapter.");
            }

            Assert.Null(nextStep);
        }
    }
}
