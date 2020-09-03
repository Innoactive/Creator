using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Configuration;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Windows
{
    /// <summary>
    /// Wizard for training course creation and management.
    /// </summary>
    internal class CourseCreationWizard : EditorWindow
    {
        private static CourseCreationWizard window;

        // CourseCreationWizard is obsolete and was replaced by CreatorSetupWizard
        // private const string menuPath = "Innoactive/Creator/Create New Course...";
#if !UNITY_2019_4_OR_NEWER || UNITY_EDITOR_OSX
        [MenuItem("Innoactive/Create New Course...")]
#endif
        private static void ShowWizard()
        {
            if (window == null)
            {
                CourseCreationWizard[] openedTrainingWizards = Resources.FindObjectsOfTypeAll<CourseCreationWizard>();

                if (openedTrainingWizards.Length > 1)
                {
                    for (int i = 1; i < openedTrainingWizards.Length; i++)
                    {
                        openedTrainingWizards[i].Close();
                    }

                    Debug.LogWarning("There were more than one create course windows open. This should not happen. The redundant windows were closed.");
                }

                window = openedTrainingWizards.Length > 0 ? openedTrainingWizards[0] : GetWindow<CourseCreationWizard>();
            }

            window.Show();
            window.Focus();
        }

        private string courseName;
        private Vector2 scrollPosition;
        private string errorMessage;

        private void OnGUI()
        {
            // Magic number.
            minSize = new Vector2(420f, 320f);
            titleContent = new GUIContent("Training Course Wizard");

            GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
            labelStyle.richText = true;
            labelStyle.wordWrap = true;

            EditorIcon logo = new EditorIcon("logo_creator");
            Rect rect = GUILayoutUtility.GetRect(position.width, 150, GUI.skin.box);
            GUI.DrawTexture(rect, logo.Texture, ScaleMode.ScaleToFit);

            if (RuntimeConfigurator.Exists == false)
            {
                EditorGUILayout.HelpBox("The current scene is not a training scene. No course can be created. To automatically setup the scene, select \"Innoactive > Setup Training Scene\".", MessageType.Error);
            }

            EditorGUI.BeginDisabledGroup(RuntimeConfigurator.Exists == false);
            EditorGUILayout.LabelField("<b>Create a new training course.</b>", labelStyle);

            courseName = EditorGUILayout.TextField(new GUIContent("Training Course Name", "Set a file name for the new training course."), courseName);

            EditorGUILayout.LabelField("The new course will be set for the current scene.");

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            // ReSharper disable once InvertIf
            if (GUILayout.Button("Create", GUILayout.Width(128), GUILayout.Height(32)))
            {
                if (CourseAssetUtils.CanCreate(courseName, out errorMessage))
                {
                    CourseAssetManager.Import(EntityFactory.CreateCourse(courseName));
                    RuntimeConfigurator.Instance.SetSelectedCourse(CourseAssetUtils.GetCourseStreamingAssetPath(courseName));
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                    GlobalEditorHandler.SetCurrentCourse(courseName);
                    GlobalEditorHandler.StartEditingCourse();

                    Close();
                }
            }

            EditorGUI.EndDisabledGroup();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            if (string.IsNullOrEmpty(errorMessage) == false)
            {
                EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
            }
        }
    }
}
