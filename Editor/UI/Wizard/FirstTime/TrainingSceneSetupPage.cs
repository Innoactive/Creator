using System.IO;
using Innoactive.CreatorEditor;
using UnityEditor;
using UnityEngine;

namespace Innoactive.Creator.Core.Editor.UI.Wizard
{
    /// <summary>
    /// Wizard page which handles the training scene setup.
    /// </summary>
    internal class TrainingSceneSetupPage : WizardPage
    {
        private bool useCurrentScene;
        private bool createNewTraining;

        private string courseName = "My first VR Training course";
        private string sceneDirectory = "Assets/Scenes";

        public TrainingSceneSetupPage() : base("Step 1: Sample Training")
        {

        }

        /// <inheritdoc />
        public override void Draw(Rect window)
        {
            Rect contentRect = DrawTitle(window, "Load a sample training");

            string spaceAfterRadioButton = "  ";

            GUILayout.BeginArea(contentRect);
            GUILayout.Space(verticalSpace);
                GUILayout.BeginHorizontal();
                GUILayout.Space(horizontalSpace);
                    GUILayout.BeginVertical();

                    createNewTraining = GUILayout.Toggle(createNewTraining, spaceAfterRadioButton + "Load sample VR training (recommended)", EditorStyles.radioButton);
                    createNewTraining = GUILayout.Toggle(!createNewTraining, spaceAfterRadioButton + "Start from scratch with an empty VR training", EditorStyles.radioButton);

                    GUILayout.Space(verticalSpace);

                    GUILayout.EndVertical();
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Space(horizontalSpace * 2);
                    GUILayout.BeginVertical();

                    GUILayout.Label("Name of your VR Training:");
                    courseName = GUILayout.TextField(courseName, 30, GUILayout.Width(contentRect.width * 0.7f));

                    string subText;

                    if (SceneExists(courseName) && useCurrentScene)
                    {
                        subText = "Scene already exists.";
                        CanProceed = false;
                    }
                    else if (CourseAssetUtils.DoesCourseAssetExist(courseName))
                    {
                        subText = "Course already exists.";
                        CanProceed = false;
                    }
                    else
                    {
                        subText = "";
                        CanProceed = true;
                    }

                    GUILayout.Label(subText, EditorStyles.whiteMiniLabel, GUILayout.MinHeight(25));

                    GUILayout.Space(verticalSpace);

                    useCurrentScene = GUILayout.Toggle(useCurrentScene, spaceAfterRadioButton + "create a new scene", EditorStyles.radioButton);
                    useCurrentScene = GUILayout.Toggle(!useCurrentScene, spaceAfterRadioButton + "take my current scene", EditorStyles.radioButton);

                    GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        /// <inheritdoc />
        public override void Apply()
        {
            base.Apply();

            if (useCurrentScene == false)
            {
                SceneSetupLogic.CreateNewScene(courseName, sceneDirectory);
            }

            SceneSetupLogic.SetupSceneAndTraining(courseName);
            EditorWindow.FocusWindowIfItsOpen<WizardWindow>();
        }

        private bool SceneExists(string sceneName)
        {
            return File.Exists($"{sceneDirectory}/{sceneName}.unity");
        }
    }
}
