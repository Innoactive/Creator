using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;
using Innoactive.Creator.Core;
using Innoactive.CreatorEditor;
using Innoactive.CreatorEditor.ImguiTester;
using Innoactive.CreatorEditor.UI.Windows;

namespace Innoactive.Creator.Tests.TrainingWindowTests
{
    /// <summary>
    /// Base class for all training window tests.
    /// </summary>
    public abstract class BaseTest : EditorImguiTest<TrainingWindow>
    {
        /// <summary>
        /// Returns all <see cref="ITransition"/>s contained in given <see cref="IStep"/>.
        /// <remarks>
        /// It also asserts that given <see cref="IStep"/> contains valid <see cref="ITransition"/>s.
        /// </remarks>
        /// </summary>
        protected static IList<ITransition> GetTransitionsFromStep(IStep step)
        {
            IList<ITransition> transitions = step.Data.Transitions.Data.Transitions;
            Assert.NotNull(transitions);
            return transitions;
        }

        /// <summary>
        /// Returns the <see cref="ICourse"/> contained in given <see cref="TrainingWindow"/>.
        /// </summary>
        protected static ICourse ExtractTraining(TrainingWindow window)
        {
            ICourse course = CourseAssetManager.TrackedCourse;
            Assert.NotNull(course);
            return course;
        }

        /// <summary>
        /// Tries to access targeted <see cref="IStep"/> from given <see cref="ITransition"/>.
        /// </summary>
        /// <param name="transition"><see cref="ITransition"/> where target <see cref="IStep"/> will be extracted.</param>
        /// <param name="step">Returned value from <see cref="ITransition"/>'s TargetStep.</param>
        /// <returns>True if given <see cref="ITransition"/> contains a target <see cref="IStep"/>, otherwise, false.</returns>
        protected static bool TryToGetStepFromTransition(ITransition transition, out IStep step)
        {
            step = transition.Data.TargetStep;
            return step != null;
        }

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
                return EditorUtils.GetCoreFolder() + "/Tests/Editor/TrainingWindow/Records";
            }
        }

        /// <inheritdoc />
        protected override TrainingWindow Given()
        {
            CourseAssetManager.Import(new Course("Test", new Chapter("Test", null)));
            TrainingWindow window = ScriptableObject.CreateInstance<TrainingWindow>();
            window.ShowUtility();
            window.position = new Rect(Vector2.zero, window.position.size);
            window.minSize = TrainingWindow.GetWindow().maxSize = new Vector2(1024f, 512f);
            CourseAssetManager.Track("Test");

            return window;
        }

        /// <inheritdoc />
        protected override void AdditionalTeardown()
        {
            CourseAssetManager.Delete(CourseAssetManager.TrackedCourse.Data.Name);
        }
    }
}
