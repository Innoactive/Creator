using System.Linq;
using NUnit.Framework;
using Innoactive.Creator.Core;
using System.Collections.Generic;
using Innoactive.CreatorEditor.UI.Windows;

namespace Innoactive.CreatorEditor.Tests.CourseWindowTests
{
    internal class AddOneStepTest : BaseTest
    {
        /// <inheritdoc />
        public override string WhenDescription
        {
            get
            {
                return "Add one step to the workflow. Connect it to the start of the chapter.";
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

            if (TryToGetStepFromTransition(transitions.First(), out IStep nextStep))
            {
                Assert.Fail("First step is not the end of the chapter.");
            }

            Assert.Null(nextStep);
        }
    }
}
