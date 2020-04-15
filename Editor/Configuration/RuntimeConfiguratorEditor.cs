using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Innoactive.Creator.Core.Configuration;
using Innoactive.Creator.Core.Utils;
using Innoactive.CreatorEditor.UI.Windows;
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
                        CourseWindow courseWindow = CourseWindow.GetWindow();
                        CourseAssetManager.Track(configurator.GetSelectedCourse());
                        courseWindow.Focus();
                    }

                    if (GUILayout.Button(new GUIContent("Show course folder in Explorer...")))
                    {
                        EditorUtility.RevealInFinder(CourseAssetManager.GetCourseAsset(CourseAssetManager.TrackedCourse.Data.Name));
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

            string courseName = Path.GetFileNameWithoutExtension(configurator.GetSelectedCourse().Split('/').Last());

            if (string.IsNullOrEmpty(courseName) == false)
            {
                index = trainingCourseDisplayNames.FindIndex(courseName.Equals);
            }

            index = EditorGUILayout.Popup("Selected Training Course", index, trainingCourseDisplayNames.ToArray());

            if (index < 0)
            {
                index = 0;
            }

            if (IsCourseListEmpty() == false)
            {
                configurator.SetSelectedCourse(CourseAssetManager.GetCourseStreamingAsset(trainingCourseDisplayNames[index]));
                CourseAssetManager.Track(trainingCourseDisplayNames[index]);
            }
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

            isDirty = false;

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

            //TODO: Why do we reset to index 0?
            CourseAssetManager.Track(trainingCourseDisplayNames[0]);
        }
    }
}
