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

            if (GUILayout.Toggle(loadSample, "Load sample VR training (recommended)", CreatorEditorStyles.Toggle)) loadSample = true;
            if (GUILayout.Toggle(!loadSample, "Start from scratch with an empty VR training", CreatorEditorStyles.Toggle)) loadSample = false;

            if (loadSample == false)
            {
                RectOffset margin = CreatorEditorStyles.Paragraph.margin;
                margin.top = CreatorEditorStyles.BaseMargin + CreatorEditorStyles.Ident;
                GUILayout.Label("Name of your VR Training:",
                    CreatorEditorStyles.ApplyMargin(CreatorEditorStyles.Paragraph, margin));
                courseName = GUILayout.TextField(courseName, 30,
                    CreatorEditorStyles.ApplyIdent(EditorStyles.textField, CreatorEditorStyles.IdentLarge),
                    GUILayout.Width(window.width * 0.7f));

                string courseInfoText = "";
                if (CourseAssetUtils.DoesCourseAssetExist(courseName))
                {
                    courseInfoText = "Course already exists and will be used.";
                }

                GUILayout.Label(courseInfoText, CreatorEditorStyles.ApplyIdent(CreatorEditorStyles.SubText, CreatorEditorStyles.IdentLarge), GUILayout.MinHeight(20));

                if (GUILayout.Toggle(useCurrentScene, "Take my current scene", CreatorEditorStyles.ApplyIdent(CreatorEditorStyles.Toggle, CreatorEditorStyles.IdentLarge))) useCurrentScene = true;
                if (GUILayout.Toggle(!useCurrentScene, "Create a new scene", CreatorEditorStyles.ApplyIdent(CreatorEditorStyles.Toggle, CreatorEditorStyles.IdentLarge))) useCurrentScene = false;

                if (useCurrentScene == false)
                {
                    string sceneInfoText = "Scene will have the same name as the training course.";
                    if (SceneExists(courseName))
                    {
                        sceneInfoText += " Scene already exists";
                        CanProceed = false;
                    }
                    else
                    {
                        CanProceed = true;
                    }

                    GUILayout.Label(sceneInfoText, CreatorEditorStyles.ApplyIdent(CreatorEditorStyles.SubText, CreatorEditorStyles.IdentLarge), GUILayout.MinHeight(20));
                }
            }

            GUILayout.EndArea();
        }

        /// <inheritdoc />
        public override void Apply()
        {
            base.Apply();

            if (useCurrentScene == false)
            {
                SceneSetupUtils.CreateNewScene(courseName, sceneDirectory);
            }

            SceneSetupUtils.SetupSceneAndTraining(courseName);
            EditorWindow.FocusWindowIfItsOpen<WizardWindow>();
        }

        private bool SceneExists(string sceneName)
        {
            return File.Exists($"{sceneDirectory}/{sceneName}.unity");
        }
    }
}
