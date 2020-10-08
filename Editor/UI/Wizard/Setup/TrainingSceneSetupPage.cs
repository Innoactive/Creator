using UnityEditor;
using UnityEngine;
using Innoactive.CreatorEditor.Setup;

namespace Innoactive.CreatorEditor.UI.Wizard
{
    /// <summary>
    /// Wizard page which handles the training scene setup.
    /// </summary>
    internal class TrainingSceneSetupPage : WizardPage
    {
        private const int MaxCourseNameLength = 40;
        private const int MinHeightOfInfoText = 30;

        [SerializeField]
        private bool useCurrentScene = true;

        [SerializeField]
        private bool createNewScene = false;

        [SerializeField]
        private bool loadSampleScene = false;

        [SerializeField]
        private string courseName = "My VR Training course";

        [SerializeField]
        private string lastCreatedCourse = null;

        private readonly GUIContent infoContent;
        private readonly GUIContent warningContent;

        public TrainingSceneSetupPage() : base("Setup Training")
        {
            infoContent = EditorGUIUtility.IconContent("console.infoicon.inactive.sml");
            warningContent = EditorGUIUtility.IconContent("console.warnicon.sml");
        }

        /// <inheritdoc />
        public override void Draw(Rect window)
        {
            GUILayout.BeginArea(window);

            GUILayout.Label("Setup Training", CreatorEditorStyles.Title);

            GUI.enabled = loadSampleScene == false;
            GUILayout.Label("Name of your VR Training", CreatorEditorStyles.Header);
            courseName = CreatorGUILayout.DrawTextField(courseName, MaxCourseNameLength, GUILayout.Width(window.width * 0.7f));
            GUI.enabled = true;

            if (CourseAssetUtils.CanCreate(courseName, out string errorMessage) == false && lastCreatedCourse != courseName)
            {
                GUIContent courseWarningContent = warningContent;
                courseWarningContent.text = errorMessage;
                GUILayout.Label(courseWarningContent, CreatorEditorStyles.Label, GUILayout.MinHeight(MinHeightOfInfoText));
                CanProceed = false;
            }
            else
            {
                GUILayout.Space(MinHeightOfInfoText + CreatorEditorStyles.BaseIndent);
                CanProceed = true;
            }

            GUILayout.BeginHorizontal();
                GUILayout.Space(CreatorEditorStyles.Indent);
                GUILayout.BeginVertical();
                bool isUseCurrentScene = GUILayout.Toggle(useCurrentScene, "Take my current scene", CreatorEditorStyles.RadioButton);
                if (useCurrentScene == false && isUseCurrentScene)
                {
                    useCurrentScene = true;
                    createNewScene = false;
                    loadSampleScene = false;
                }

                bool isCreateNewScene = GUILayout.Toggle(createNewScene, "Create a new scene", CreatorEditorStyles.RadioButton);
                if (createNewScene == false && isCreateNewScene)
                {
                    createNewScene = true;
                    useCurrentScene = false;
                    loadSampleScene = false;
                }

                EditorGUILayout.Space();

                loadSampleScene = GUILayout.Toggle(loadSampleScene, "Load Step by Step Guide Scene", CreatorEditorStyles.RadioButton);
                if (loadSampleScene)
                {
                    createNewScene = false;
                    useCurrentScene = false;

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Space(CreatorEditorStyles.Indent);
                        CreatorGUILayout.DrawLink("Hello Creator – a 5-step guide to a basic training application", "https://developers.innoactive.de/documentation/creator/latest/articles/step-by-step-guides/hello-creator.html");
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            if (createNewScene)
            {
                GUIContent helpContent;
                string sceneInfoText = "Scene will have the same name as the training course.";
                if (SceneSetupUtils.SceneExists(courseName))
                {
                    sceneInfoText += " Scene already exists";
                    CanProceed = false;
                    helpContent = warningContent;
                }
                else
                {
                    helpContent = infoContent;
                }

                helpContent.text = sceneInfoText;
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Space(CreatorEditorStyles.Indent);
                    EditorGUILayout.LabelField(helpContent, CreatorEditorStyles.Label, GUILayout.MinHeight(MinHeightOfInfoText));
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.EndArea();
        }

        /// <inheritdoc />
        public override void Apply()
        {
            if (courseName == lastCreatedCourse)
            {
                return;
            }

            if (loadSampleScene)
            {
                SceneSetupUtils.CreateNewSimpleExampleScene();
                return;
            }

            if (useCurrentScene == false)
            {
                SceneSetupUtils.CreateNewScene(courseName);
            }

            SceneSetupUtils.SetupSceneAndTraining(courseName);
            lastCreatedCourse = courseName;
            EditorWindow.FocusWindowIfItsOpen<WizardWindow>();
        }
    }
}
