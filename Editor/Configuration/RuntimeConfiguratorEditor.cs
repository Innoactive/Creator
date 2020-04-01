using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Innoactive.Creator.Core.Configuration;
using Innoactive.Creator.Core.Utils;
using Innoactive.CreatorEditor.UI.Windows;
using Innoactive.CreatorEditor.Utils;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.Configuration
{
    /// <summary>
    /// Custom editor for choosing the training configuration in the Unity game object inspector.
    /// </summary>
    [CustomEditor(typeof(RuntimeConfigurator))]
    public class RuntimeConfiguratorEditor : Editor
    {
        private RuntimeConfigurator configurator;

        private static List<Type> configurationTypes;
        private static string[] configurationTypeNames;

        private static List<string> trainingCourseDisplayNames = new List<string> {"<none>"};

        private string defaultCoursePath;
        private static bool isDirty = true;

        static RuntimeConfiguratorEditor()
        {
            configurationTypes = ReflectionUtils.GetConcreteImplementationsOf<IRuntimeConfiguration>().ToList();
            configurationTypes.Sort(((type1, type2) => String.Compare(type1.Name, type2.Name, StringComparison.Ordinal)));
            configurationTypeNames = configurationTypes.Select(t => t.Name).ToArray();

            CourseAssetPostprocessor.CourseFileStructureChanged += OnCourseFileStructureChanged;
        }

        /// <summary>
        /// True when the training course list is empty or missing.
        /// </summary>
        public static bool IsCourseListEmpty()
        {
            return trainingCourseDisplayNames.Count == 1 && trainingCourseDisplayNames[0] == "<none>";
        }

        protected void OnEnable()
        {
            configurator = target as RuntimeConfigurator;
            defaultCoursePath = EditorConfigurator.Instance.DefaultCourseStreamingAssetsFolder;

            // Create training course path if not present.
            string absolutePath = Path.Combine(Application.streamingAssetsPath, defaultCoursePath);
            if (Directory.Exists(absolutePath) == false)
            {
                Directory.CreateDirectory(absolutePath);
            }
        }

        public override void OnInspectorGUI()
        {
            // Courses can change without recompile so we have to check for them.
            UpdateAvailableCourses();

            DrawRuntimeConfigurationDropDown();

            EditorGUI.BeginDisabledGroup(IsCourseListEmpty());
            {
                DrawCourseSelectionDropDown();
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Open course in Workflow Editor"))
                    {
                        TrainingWindow trainingWindow = TrainingWindow.GetWindow();
                        trainingWindow.LoadTrainingCourseFromFile(GetSelectedCourseAbsolutePath());
                        trainingWindow.Focus();
                    }

                    if (GUILayout.Button(new GUIContent("Show course folder in Explorer...")))
                    {
                        EditorUtility.RevealInFinder(Path.GetDirectoryName(GetSelectedCourseAbsolutePath()));
                    }
                }
                GUILayout.EndHorizontal();
            }
            EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawRuntimeConfigurationDropDown()
        {
            int index = configurationTypes.FindIndex(t =>
                t.AssemblyQualifiedName == configurator.GetRuntimeConfigurationName());
            index = EditorGUILayout.Popup("Configuration", index, configurationTypeNames);
            configurator.SetRuntimeConfigurationName(configurationTypes[index].AssemblyQualifiedName);
        }

        private void DrawCourseSelectionDropDown()
        {
            int index = 0;
            string course = EditorCourseUtils.GetActiveCourseName();
            if (string.IsNullOrEmpty(course) == false)
            {
                index = trainingCourseDisplayNames.FindIndex(course.Equals);
            }

            index = EditorGUILayout.Popup("Selected Training Course", index, trainingCourseDisplayNames.ToArray());

            if (index < 0)
            {
                index = 0;
            }

            configurator.SetSelectedTrainingCourse(EditorCourseUtils.GetCoursePath(trainingCourseDisplayNames[index]));
        }

        private static void OnCourseFileStructureChanged(object sender, CourseAssetPostprocessorEventArgs args)
        {
            isDirty = true;
        }

        private string GetSelectedCourseAbsolutePath()
        {
            return EditorCourseUtils.GetAbsoluteCoursePath(configurator.GetSelectedTrainingCourse());
        }

        private void UpdateAvailableCourses()
        {
            if (isDirty == false)
            {
                return;
            }
            isDirty = false;


            List<string> courses = Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, defaultCoursePath).Replace('/', Path.DirectorySeparatorChar), "*.json", SearchOption.AllDirectories)
                .Where(FileUtils.IsCourseFile)
                .Select(path => path.Substring(Application.streamingAssetsPath.Length + 1))
                .ToList();

            // Create dummy entry if no files are present.
            if (courses.Count == 0)
            {
                trainingCourseDisplayNames.Clear();
                trainingCourseDisplayNames.Add("<none>");
                return;
            }

            trainingCourseDisplayNames = courses.Select(Path.GetFileNameWithoutExtension).ToList();
            trainingCourseDisplayNames.Sort();

            if (string.IsNullOrEmpty(configurator.GetSelectedTrainingCourse()))
            {
                configurator.SetSelectedTrainingCourse(EditorCourseUtils.GetCoursePath(trainingCourseDisplayNames[0]));
            }
        }
    }
}
