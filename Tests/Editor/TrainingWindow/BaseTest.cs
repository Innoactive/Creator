using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;
using Innoactive.Hub.Training;
using Innoactive.Hub.Training.Editors.Utils;
using Innoactive.Hub.Training.Editors.Windows;
using Innoactive.Hub.Training.Unity.Utils;
using Innoactive.Hub.Unity.Tests.Training.Editor.EditorImguiTester;

namespace Innoactive.Hub.Unity.Tests.Training.Editor.Windows.TrainingWindowTests
{
    /// <summary>
    /// Base class for all training window tests.
    /// </summary>
    public abstract class BaseTest : EditorImguiTest<TrainingWindow>
    {
        /// <inheritdoc />
        public override string GivenDescription
        {
            get
            {
                return "A training window with empty training and fixed size of 1024x512 pixels.";
            }
        }

        /// <inheritdoc />
        protected override string AssetFolderForRecordedActions
        {
            get
            {
                return EditorUtils.GetModuleFolder() + "/Editor/Tests/Windows/TrainingWindow/Records";
            }
        }

        /// <inheritdoc />
        protected override TrainingWindow Given()
        {
            SceneUtils.SetupTrainingConfiguration();

            TrainingWindow window = ScriptableObject.CreateInstance<TrainingWindow>();
            window.ShowUtility();
            window.position = new Rect(Vector2.zero, window.position.size);
            window.minSize = TrainingWindow.GetWindow().maxSize = new Vector2(1024f, 512f);
            window.SetTrainingCourseWithUserConfirmation(new Course("Test", new Chapter("Test", null)));

            return window;
        }

        /// <summary>
        /// Returns the <see cref="ICourse"/> contained in given <see cref="TrainingWindow"/>.
        /// </summary>
        protected ICourse ExtractTraining(TrainingWindow window)
        {
            ICourse course = window.GetTrainingCourse();
            Assert.NotNull(course);
            return course;
        }

        /// <summary>
        /// Returns all <see cref="ITransition"/>s contained in given <see cref="IStep"/>.
        /// <remarks>
        /// It also asserts that given <see cref="IStep"/> contains valid <see cref="ITransition"/>s.
        /// </remarks>
        /// </summary>
        protected IList<ITransition> GetTransitionsFromStep(IStep step)
        {
            IList<ITransition> transitions = step.Data.Transitions.Data.Transitions;
            Assert.NotNull(transitions);
            return transitions;
        }

        /// <summary>
        /// Tries to access targeted <see cref="IStep"/> from given <see cref="ITransition"/>.
        /// </summary>
        /// <param name="transition"><see cref="ITransition"/> where target <see cref="IStep"/> will be extracted.</param>
        /// <param name="step">Returned value from <see cref="ITransition"/>'s TargetStep.</param>
        /// <returns>True if given <see cref="ITransition"/> contains a target <see cref="IStep"/>, otherwise, false.</returns>
        protected bool TryToGetStepFromTransition(ITransition transition, out IStep step)
        {
            step = transition.Data.TargetStep;
            return step != null;
        }
    }
}
