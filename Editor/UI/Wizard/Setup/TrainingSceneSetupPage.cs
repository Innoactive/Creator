using UnityEditor;
using UnityEngine;
using VPG.Editor.Setup;

namespace VPG.Editor.UI.Wizard
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

            GUILayout.Label("Setup Training", VPGEditorStyles.Title);

            GUI.enabled = loadSampleScene == false;
            GUILayout.Label("Name of your VR Training", VPGEditorStyles.Header);
            courseName = VPGGUILayout.DrawTextField(courseName, MaxCourseNameLength, GUILayout.Width(window.width * 0.7f));
            GUI.enabled = true;

            if (CourseAssetUtils.CanCreate(courseName, out string errorMessage) == false && lastCreatedCourse != courseName)
            {
                GUIContent courseWarningContent = warningContent;
                courseWarningContent.text = errorMessage;
                GUILayout.Label(courseWarningContent, VPGEditorStyles.Label, GUILayout.MinHeight(MinHeightOfInfoText));
                CanProceed = false;
            }
            else
            {
                GUILayout.Space(MinHeightOfInfoText + VPGEditorStyles.BaseIndent);
                CanProceed = true;
            }

            GUILayout.BeginHorizontal();
                GUILayout.Space(VPGEditorStyles.Indent);
                GUILayout.BeginVertical();
                bool isUseCurrentScene = GUILayout.Toggle(useCurrentScene, "Take my current scene", VPGEditorStyles.RadioButton);
                if (useCurrentScene == false && isUseCurrentScene)
                {
                    useCurrentScene = true;
                    createNewScene = false;
                    loadSampleScene = false;
                }

                bool isCreateNewScene = GUILayout.Toggle(createNewScene, "Create a new scene", VPGEditorStyles.RadioButton);
                if (createNewScene == false && isCreateNewScene)
                {
                    createNewScene = true;
                    useCurrentScene = false;
                    loadSampleScene = false;
                }

                EditorGUILayout.Space();

                loadSampleScene = GUILayout.Toggle(loadSampleScene, "Load Step by Step Guide Scene", VPGEditorStyles.RadioButton);
                if (loadSampleScene)
                {
                    createNewScene = false;
                    useCurrentScene = false;
                    CanProceed = true;

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Space(VPGEditorStyles.Indent);
                        VPGGUILayout.DrawLink("Hello Creator – a 5-step guide to a basic training application", "https://developers.innoactive.de/documentation/creator/latest/articles/step-by-step-guides/hello-creator.html");
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
                    GUILayout.Space(VPGEditorStyles.Indent);
                    EditorGUILayout.LabelField(helpContent, VPGEditorStyles.Label, GUILayout.MinHeight(MinHeightOfInfoText));
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
