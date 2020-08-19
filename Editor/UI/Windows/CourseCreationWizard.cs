using System.Collections.Generic;
using System.IO;
using System.Linq;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Configuration;
using Innoactive.CreatorEditor.Setup;
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
        private const string menuPath = "Innoactive/Creator/Open Course...";

        protected EditorIcon logo; // = new EditorIcon("logo_creator_icon");

        private int selectedCourseIndex = 0;

        [MenuItem(menuPath, false, 12)]
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
            Vector2 windowSize = new Vector2(420f, 250f);
            minSize = windowSize;
            maxSize = windowSize;
            titleContent = new GUIContent("Open Course");

            Rect drawingWindow = new Rect(0f, 10f, windowSize.x, windowSize.y);
            GUILayout.BeginArea(drawingWindow, CreatorEditorStyles.Paragraph);

            if (logo == null)
            {
                logo = new EditorIcon("logo_creator_icon");
            }

            Rect logoRect = new Rect(drawingWindow.x + drawingWindow.width * 0.25f, drawingWindow.y, drawingWindow.width * 0.5f, (drawingWindow.width * 0.34f) * 0.5f);
            GUILayout.BeginArea(logoRect);
                GUI.DrawTexture(new Rect(0, 0, logoRect.width, logoRect.height), logo.Texture);
            GUILayout.EndArea();

            Rect contentArea = drawingWindow;
            contentArea.y = logoRect.height + CreatorEditorStyles.Indent * 2;
            contentArea.height -= logoRect.height;

            bool disableCourseSelection = false;
            bool doesAnyCourseExist = CourseAssetUtils.DoesAnyCourseExist();
            bool isSceneSetup = RuntimeConfigurator.Exists;

            if (doesAnyCourseExist == false || isSceneSetup == false)
            {
                disableCourseSelection = true;
                string errorMessage = (doesAnyCourseExist == false)
                    ? "There are no training courses in this Unity project. To create a course select \"Innoactive > Creator > Create New Course...\"."
                    : "The current scene is not a training scene. No course can be created. To automatically setup the scene, select \"Innoactive > Creator > Setup Current Scene as Training Scene\".";

                // Add offset to HelpBox Area which cannot be styled with EditorStyles
                RectOffset offset = new RectOffset(CreatorEditorStyles.Indent, CreatorEditorStyles.Indent, 0, 0);
                contentArea = offset.Remove(contentArea);
                GUILayout.BeginArea(contentArea);
                    EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
                GUILayout.EndArea();

                // Reset the offset
                contentArea = new RectOffset(CreatorEditorStyles.Indent, CreatorEditorStyles.Indent, -50, 0).Add(contentArea);
            }

            GUILayout.BeginArea(contentArea);
            EditorGUI.BeginDisabledGroup(disableCourseSelection);

            GUILayout.Label("Choose course to use in current scene:", CreatorEditorStyles.Paragraph);

            string[] courseNames = CourseAssetUtils.DoesAnyCourseExist() ? CourseAssetUtils.GetAllCourses().ToArray() : new [] { "" };
            selectedCourseIndex = EditorGUILayout.Popup(selectedCourseIndex, courseNames, CreatorEditorStyles.Popup);

            GUILayout.Space(CreatorEditorStyles.Indent);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
                if (GUILayout.Button("Open Course", GUILayout.Width(128), GUILayout.Height(32)))
                {
                    SceneSetupUtils.SetCourseInCurrentScene(courseNames[selectedCourseIndex]);
                }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            EditorGUI.EndDisabledGroup();
            GUILayout.EndArea();

            GUILayout.EndArea();
        }
    }
}
