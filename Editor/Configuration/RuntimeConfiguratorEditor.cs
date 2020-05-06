using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Innoactive.Creator.Core.Configuration;
using Innoactive.Creator.Core.Utils;
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

        private static readonly List<Type> configurationTypes;
        private static readonly string[] configurationTypeNames;

        private static List<string> trainingCourseDisplayNames = new List<string> { "<none>" };

        private string defaultCoursePath;
        private static bool isDirty = true;

        static RuntimeConfiguratorEditor()
        {
            configurationTypes = ReflectionUtils.GetConcreteImplementationsOf<IRuntimeConfiguration>().ToList();
            configurationTypes.Sort(((type1, type2) => string.Compare(type1.Name, type2.Name, StringComparison.Ordinal)));
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
            defaultCoursePath = EditorConfigurator.Instance.CourseStreamingAssetsSubdirectory;

            // Create training course path if not present.
            string absolutePath = Path.Combine(Application.streamingAssetsPath, defaultCoursePath);
            if (Directory.Exists(absolutePath) == false)
            {
                Directory.CreateDirectory(absolutePath);
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Courses can change without recompile so we have to check for them.
            UpdateAvailableCourses();

            DrawRuntimeConfigurationDropDown();

            EditorGUI.BeginDisabledGroup(IsCourseListEmpty());
            {
                DrawCourseSelectionDropDown();
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Open Course in Workflow Editor"))
                    {
                        Editors.SetCurrentCourse(CourseAssetManager.GetCourseNameFromPath(configurator.GetSelectedCourse()));
                        Editors.StartEditingCourse();
                    }

                    if (GUILayout.Button(new GUIContent("Show Course in Explorer...")))
                    {
                        string absolutePath = $"{new FileInfo(CourseAssetManager.GetCourseAssetPath(CourseAssetManager.GetCourseNameFromPath(configurator.GetSelectedCourse())))}";
                        EditorUtility.RevealInFinder(absolutePath);
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

            string courseName = CourseAssetManager.GetCourseNameFromPath(configurator.GetSelectedCourse());

            if (string.IsNullOrEmpty(courseName) == false)
            {
                index = trainingCourseDisplayNames.FindIndex(courseName.Equals);
            }

            index = EditorGUILayout.Popup("Selected Training Course", index, trainingCourseDisplayNames.ToArray());

            if (index < 0)
            {
                index = 0;
            }

            string newCourseStreamingAssetsPath = CourseAssetManager.GetCourseStreamingAssetPath(trainingCourseDisplayNames[index]);

            if (IsCourseListEmpty() == false && configurator.GetSelectedCourse() != newCourseStreamingAssetsPath)
            {

                SetConfiguratorSelectedCourse(newCourseStreamingAssetsPath);
                Editors.SetCurrentCourse(trainingCourseDisplayNames[index]);
            }
        }

        private void SetConfiguratorSelectedCourse(string newPath)
        {
            serializedObject.FindProperty("selectedCourseStreamingAssetsPath").stringValue = newPath;
        }

        private static void OnCourseFileStructureChanged(object sender, CourseAssetPostprocessorEventArgs args)
        {
            isDirty = true;
        }

        private void UpdateAvailableCourses()
        {
            if (isDirty == false)
            {
                return;
            }

            List<string> courses = CourseAssetManager.GetAllCourses().ToList();

            // Create dummy entry if no files are present.
            if (courses.Any() == false)
            {
                trainingCourseDisplayNames.Clear();
                trainingCourseDisplayNames.Add("<none>");
                return;
            }

            trainingCourseDisplayNames = courses;
            trainingCourseDisplayNames.Sort();

            if (string.IsNullOrEmpty(configurator.GetSelectedCourse()))
            {
                SetConfiguratorSelectedCourse(CourseAssetManager.GetCourseStreamingAssetPath(trainingCourseDisplayNames[0]));
                Editors.SetCurrentCourse(CourseAssetManager.GetCourseAssetPath(configurator.GetSelectedCourse()));
            }
        }
    }
}
