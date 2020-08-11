using UnityEditor;
using UnityEngine;

namespace Innoactive.Creator.Core.Editor.UI.Wizard
{
    internal class TrainingSceneSetupPage : WizardPage
    {
        private bool createNewScene;
        private bool importSampleTraining;

        private string courseName = "My first VR Training course";

        public TrainingSceneSetupPage() : base("Step 1: Sample Training")
        {

        }

        public override void Draw(Rect window)
        {
            Rect contentRect = DrawTitle(window, "Load a sample training");

            string spaceAfterRadioButton = "  ";

            GUILayout.BeginArea(contentRect);
            GUILayout.Space(verticalSpace);
                GUILayout.BeginHorizontal();
                GUILayout.Space(horizontalSpace);
                    GUILayout.BeginVertical();

                    importSampleTraining = GUILayout.Toggle(importSampleTraining, spaceAfterRadioButton + "Load sample VR training (recommended)", EditorStyles.radioButton);
                    importSampleTraining = GUILayout.Toggle(!importSampleTraining, spaceAfterRadioButton + "Start from scratch with an empty VR training", EditorStyles.radioButton);

                    GUILayout.Space(verticalSpace);

                    GUILayout.EndVertical();
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Space(horizontalSpace * 2);
                    GUILayout.BeginVertical();

                    GUILayout.Label("Name of your VR Training:");
                    courseName = GUILayout.TextField(courseName, 30, GUILayout.Width(contentRect.width * 0.7f));

                    GUILayout.Space(verticalSpace);

                    createNewScene = GUILayout.Toggle(createNewScene, spaceAfterRadioButton + "create a new scene", EditorStyles.radioButton);
                    createNewScene = GUILayout.Toggle(!createNewScene, spaceAfterRadioButton + "take my current scene", EditorStyles.radioButton);

                    GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        public override void Apply()
        {
            base.Apply();

            if (createNewScene)
            {
                SceneSetupLogic.CreateNewScene(courseName);
            }

        }
    }
}
