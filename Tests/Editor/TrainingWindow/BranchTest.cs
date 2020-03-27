using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Innoactive.Creator.Core;
using Innoactive.CreatorEditor.UI.Windows;

namespace Innoactive.Creator.Tests.TrainingWindowTests
{
    public class BranchTest : BaseTest
    {
        /// <inheritdoc />
        public override string WhenDescription
        {
            get
            {
                return "User creates a step with two transitions, each goes to another step each with one transition to the end of the chapter.";
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
        protected override void Then(TrainingWindow window)
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
