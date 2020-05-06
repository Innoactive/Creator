using Innoactive.CreatorEditor.UI.Windows;
using Innoactive.CreatorEditor.TestTools;
using NUnit.Framework;
using UnityEngine;

namespace Innoactive.CreatorEditor.Tests.CourseWizardTests
{
    internal class CreateCourseTest : EditorImguiTest<CourseCreationWizard>
    {
        private const string courseName = "very_unique_test_course_name_which_you_should_never_use_1534";
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
                return $"Type in \"{courseName}\". Click 'Create training' button.";
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

        protected override CourseCreationWizard Given()
        {
            Editors.SetStrategy(new EmptyTestStrategy());

            CourseAssetManager.Delete(courseName);

            foreach (CourseCreationWizard window in Resources.FindObjectsOfTypeAll<CourseCreationWizard>())
            {
                window.Close();
            }

            CourseCreationWizard wizard = ScriptableObject.CreateInstance<CourseCreationWizard>();
            wizard.ShowUtility();
            wizard.maxSize = wizard.minSize;
            wizard.position = new Rect(Vector2.zero, wizard.minSize);
            return wizard;
        }

        protected override void Then(CourseCreationWizard window)
        {
            Assert.False(EditorUtils.IsWindowOpened<CourseWindow>());
        }

        protected override void AdditionalTeardown()
        {
            base.AdditionalTeardown();
            foreach (CourseCreationWizard window in Resources.FindObjectsOfTypeAll<CourseCreationWizard>())
            {
                window.Close();
            }

            CourseAssetManager.Delete(courseName);
            Editors.SetDefaultStrategy();
        }
    }
}
