using Innoactive.CreatorEditor;
using Innoactive.CreatorEditor.UI.Windows;
using Innoactive.CreatorEditor.ImguiTester;
using NUnit.Framework;
using UnityEngine;

namespace Innoactive.Creator.Tests.TrainingWizardTests
{
    public class CreateTrainingTest : EditorImguiTest<CreatingCourseWizard>
    {
        public override string GivenDescription
        {
            get
            {
                return "Opened training wizard window.";
            }
        }

        public override string WhenDescription
        {
            get
            {
                return "Click 'Create training' button.";
            }
        }

        public override string ThenDescription
        {
            get
            {
                return "Training window is opened.";
            }
        }

        protected override string AssetFolderForRecordedActions
        {
            get
            {
                return EditorUtils.GetCoreFolder() + "/Tests/Editor/TrainingWizard/Records";
            }
        }

        protected override CreatingCourseWizard Given()
        {
            foreach (CreatingCourseWizard window in Resources.FindObjectsOfTypeAll<CreatingCourseWizard>())
            {
                window.Close();
            }

            foreach (CourseWindow window in Resources.FindObjectsOfTypeAll<CourseWindow>())
            {
                window.Close();
            }

            CreatingCourseWizard wizard = ScriptableObject.CreateInstance<CreatingCourseWizard>();
            wizard.ShowUtility();
            wizard.maxSize = wizard.minSize;
            wizard.position = new Rect(Vector2.zero, wizard.minSize);
            return wizard;
        }

        protected override void Then(CreatingCourseWizard window)
        {
            Assert.False(EditorUtils.IsWindowOpened<CourseWindow>());
        }

        protected override void AdditionalTeardown()
        {
            base.AdditionalTeardown();
            foreach (CreatingCourseWizard window in Resources.FindObjectsOfTypeAll<CreatingCourseWizard>())
            {
                window.Close();
            }

            foreach (CourseWindow window in Resources.FindObjectsOfTypeAll<CourseWindow>())
            {
                window.Close();
            }
        }
    }
}
