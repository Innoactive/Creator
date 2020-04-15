using Innoactive.CreatorEditor;
using Innoactive.CreatorEditor.UI.Windows;
using Innoactive.CreatorEditor.ImguiTester;
using NUnit.Framework;
using UnityEngine;

namespace Innoactive.Creator.Tests.TrainingWizardTests
{
    public class CreateTrainingTest : EditorImguiTest<TrainingWizard>
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

        protected override TrainingWizard Given()
        {
            foreach (TrainingWizard window in Resources.FindObjectsOfTypeAll<TrainingWizard>())
            {
                window.Close();
            }

            foreach (TrainingWindow window in Resources.FindObjectsOfTypeAll<TrainingWindow>())
            {
                window.Close();
            }

            TrainingWizard wizard = ScriptableObject.CreateInstance<TrainingWizard>();
            wizard.ShowUtility();
            wizard.maxSize = wizard.minSize;
            wizard.position = new Rect(Vector2.zero, wizard.minSize);
            return wizard;
        }

        protected override void Then(TrainingWizard window)
        {
            Assert.False(EditorUtils.IsWindowOpened<TrainingWindow>());
        }

        protected override void AdditionalTeardown()
        {
            base.AdditionalTeardown();
            foreach (TrainingWizard window in Resources.FindObjectsOfTypeAll<TrainingWizard>())
            {
                window.Close();
            }

            foreach (TrainingWindow window in Resources.FindObjectsOfTypeAll<TrainingWindow>())
            {
                window.Close();
            }
        }
    }
}
