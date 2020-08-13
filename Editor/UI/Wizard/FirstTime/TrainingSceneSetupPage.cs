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
        private bool useCurrentScene = true;
        private bool createNewTraining = false;
        private bool loadSample = true;

        private string courseName = "My first VR Training course";
        private string sceneDirectory = "Assets/Scenes";

        public TrainingSceneSetupPage() : base("Step 1: Sample Training")
        {

        }

        /// <inheritdoc />
        public override void Draw(Rect window)
        {
            GUILayout.BeginArea(window);

            GUILayout.Label("Load a sample training", CreatorEditorStyles.Title);

            loadSample = GUILayout.Toggle(!createNewTraining, "Load sample VR training (recommended)", CreatorEditorStyles.RadioButton);
            createNewTraining = GUILayout.Toggle(!loadSample, "Start from scratch with an empty VR training", CreatorEditorStyles.RadioButton);

            if (createNewTraining)
            {
                RectOffset margin = CreatorEditorStyles.Paragraph.margin;
                margin.top = CreatorEditorStyles.BaseMargin + CreatorEditorStyles.Ident;
                GUILayout.Label("Name of your VR Training:",
                    CreatorEditorStyles.ApplyMargin(CreatorEditorStyles.Paragraph, margin));
                courseName = GUILayout.TextField(courseName, 30,
                    CreatorEditorStyles.ApplyIdent(EditorStyles.textField, CreatorEditorStyles.IdentLarge),
                    GUILayout.Width(window.width * 0.7f));

                string subText;

                if (SceneExists(courseName) && useCurrentScene)
                {
                    subText = "Scene already exists.";
                    CanProceed = false;
                }
                else if (CourseAssetUtils.DoesCourseAssetExist(courseName))
                {
                    subText = "Course already exists and will be used.";
                    CanProceed = true;
                }
                else
                {
                    subText = "";
                    CanProceed = true;
                }

                GUILayout.Label(subText, CreatorEditorStyles.SubText, GUILayout.MinHeight(25));

                useCurrentScene = GUILayout.Toggle(useCurrentScene, "Take my current scene",
                    CreatorEditorStyles.ApplyIdent(CreatorEditorStyles.RadioButton, CreatorEditorStyles.IdentLarge));

                useCurrentScene = GUILayout.Toggle(!useCurrentScene, "Create a new scene",
                    CreatorEditorStyles.ApplyIdent(CreatorEditorStyles.RadioButton, CreatorEditorStyles.IdentLarge));
            }

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
