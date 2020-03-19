using System.IO;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Configuration;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Windows
{
    /// <summary>
    /// Wizard for training course creation and management.
    /// </summary>
    public class TrainingWizard : EditorWindow
    {
        private static TrainingWizard window;
        private const string menuPath = "Innoactive/Creator/Create New Course...";

        [MenuItem(menuPath, false, 12)]
        private static void ShowWizard()
        {
            if (window == null)
            {
                TrainingWizard[] openedTrainingWizards = Resources.FindObjectsOfTypeAll<TrainingWizard>();

                if (openedTrainingWizards.Length > 1)
                {
                    for (int i = 1; i < openedTrainingWizards.Length; i++)
                    {
                        openedTrainingWizards[i].Close();
                    }
                    Debug.LogWarning("There were more than one create course windows open. This should not happen. The redundant windows were closed.");
                }

                window = openedTrainingWizards.Length > 0 ? openedTrainingWizards[0] : GetWindow<TrainingWizard>();
            }

            window.Show();
            window.Focus();
        }

        private string trainingName;
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
                EditorGUILayout.HelpBox("The current scene is not a training scene. No course can be created. To automatically setup the scene, select \"Innoactive > Training > Setup Current Scene as Training Scene\".", MessageType.Error);
            }

            EditorGUI.BeginDisabledGroup(RuntimeConfigurator.Exists == false);
            EditorGUILayout.LabelField("<b>Create a new training course.</b>", labelStyle);

            trainingName = EditorGUILayout.TextField(new GUIContent("Training Course Name", "Set a file name for the new training course."), trainingName);

            EditorGUILayout.LabelField("The new course will be set for the current scene.");

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            // ReSharper disable once InvertIf
            if (GUILayout.Button("Create", GUILayout.Width(128), GUILayout.Height(32)))
            {
                int invalidCharacterIndex;

                if (string.IsNullOrEmpty(trainingName))
                {
                    errorMessage = "Training course name is empty!";
                }
                else if ((invalidCharacterIndex = trainingName.IndexOfAny(Path.GetInvalidFileNameChars())) >= 0)
                {
                    errorMessage = string.Format("Course name contains invalid character: {0}", trainingName[invalidCharacterIndex]);
                }
                else
                {
                    string trainingCoursePath = EditorCourseUtils.GetTrainingPath(trainingName);
                    string trainingCourseFolder = Path.GetDirectoryName(trainingCoursePath);

                    if (Directory.Exists(trainingCourseFolder))
                    {
                        errorMessage = string.Format("Training course with name \"{0}\" already exists!", trainingName);
                    }
                    else
                    {
                        ICourse course = EditorCourseUtils.CreateCourse(trainingName);
                        if (EditorCourseUtils.SetTrainingCourseActive(course))
                        {
                            Debug.Log("Newly created course saved.");
                            Close();
                        }
                    }
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
