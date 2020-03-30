using System;
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
        private SerializedProperty configurationName;
        private SerializedProperty selectedCourseStreamingAssetsPath;
        private static readonly string[] configurationTypeNames;
        private static readonly string[] shortConfigurationTypeNames;
        private static string[] trainingCourseStreamingAssetsPaths;
        private static string[] trainingCourseDisplayNames;
        private int selectedConfigurationIndex;
        private int selectedCourseIndex;
        private string selectedCourse;
        private string selectedConfiguration;
        private string defaultCoursePath;
        private static bool isDirty = true;

        static RuntimeConfiguratorEditor()
        {
            configurationTypeNames = ReflectionUtils.GetConcreteImplementationsOf<IRuntimeConfiguration>()
                .Select(type => type.AssemblyQualifiedName)
                .ToArray();

            shortConfigurationTypeNames = configurationTypeNames.Select(assemblyQualifiedName =>
            {
                string fullName = assemblyQualifiedName.Substring(0, assemblyQualifiedName.IndexOf(','));
                int lastIndexPeriod = fullName.LastIndexOf('.') + 1;
                Mathf.Clamp(lastIndexPeriod, 0, fullName.Length - 1);
                string shortName = fullName.Substring(lastIndexPeriod).Replace('+', '.');
                return string.Format("{0} ({1})", shortName, fullName);
            }).ToArray();

            CourseAssetPostprocessor.CourseFileStructureChanged += OnCourseFileStructureChanged;
        }

        /// <summary>
        /// True when the training course list is empty or missing.
        /// </summary>
        public static bool IsCourseListEmpty()
        {
            return trainingCourseStreamingAssetsPaths == null || trainingCourseStreamingAssetsPaths.Length <= 0;
        }

        protected void OnEnable()
        {
            configurationName = serializedObject.FindProperty("runtimeConfigurationName");
            selectedCourseStreamingAssetsPath = serializedObject.FindProperty("selectedCourseStreamingAssetsPath");
            defaultCoursePath = EditorConfigurator.Instance.DefaultCourseStreamingAssetsFolder;

            // Create training course path if not present.
            string absolutePath = Path.Combine(Application.streamingAssetsPath, defaultCoursePath);
            if (Directory.Exists(absolutePath) == false)
            {
                Directory.CreateDirectory(absolutePath);
            }

            UpdateAvailableCourses();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            UpdateSelectedConfigurationName();
            UpdateSelectedCoursePath();

            selectedConfigurationIndex = EditorGUILayout.Popup("Configuration", selectedConfigurationIndex, shortConfigurationTypeNames);
            configurationName.stringValue = configurationTypeNames[selectedConfigurationIndex];

            selectedCourseIndex = EditorGUILayout.Popup("Selected Training Course", selectedCourseIndex, trainingCourseDisplayNames);

            if (trainingCourseStreamingAssetsPaths.Length > 0)
            {
                selectedCourseStreamingAssetsPath.stringValue = trainingCourseStreamingAssetsPaths[selectedCourseIndex];
            }

            EditorGUI.BeginDisabledGroup(IsCourseListEmpty());
            {
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Open course in Workflow Editor"))
                    {
                        TrainingWindow trainingWindow = TrainingWindow.GetWindow();

                        trainingWindow.LoadTrainingCourseFromFile(GetSelectedCourseAbsolutePath());

                        trainingWindow.Focus();
                    }

                    if (GUILayout.Button(new GUIContent("Show course folder in Explorer...", Path.GetDirectoryName(GetSelectedCourseAbsolutePath()))))
                    {
                        EditorUtility.RevealInFinder(Path.GetDirectoryName(GetSelectedCourseAbsolutePath()));
                    }
                }
                GUILayout.EndHorizontal();
            }
            EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties();
        }

        private static void OnCourseFileStructureChanged(object sender, CourseAssetPostprocessorEventArgs args)
        {
            isDirty = true;
        }

        private string GetSelectedCourseAbsolutePath()
        {
            return EditorCourseUtils.GetTrainingPath(selectedCourseStreamingAssetsPath.stringValue);
        }

        private void UpdateSelectedConfigurationName()
        {
            if (configurationName.stringValue != selectedConfiguration)
            {
                selectedConfigurationIndex = Array.IndexOf(configurationTypeNames, configurationName.stringValue);

                if (selectedConfigurationIndex < 0)
                {
                    selectedConfigurationIndex = Array.IndexOf(configurationTypeNames, typeof(DefaultRuntimeConfiguration).AssemblyQualifiedName);
                }

                selectedConfiguration = configurationName.stringValue;
            }
        }

        private void UpdateSelectedCoursePath()
        {
            string currentCoursePath = selectedCourseStreamingAssetsPath.stringValue;

            if (isDirty == false && (string.IsNullOrEmpty(currentCoursePath) || currentCoursePath == selectedCourse))
            {
                return;
            }

            isDirty = false;
            UpdateAvailableCourses();
            selectedCourseIndex = Array.IndexOf(trainingCourseStreamingAssetsPaths, currentCoursePath);

            if (selectedCourseIndex < 0)
            {
                selectedCourseIndex = 0;
            }

            selectedCourse = selectedCourseStreamingAssetsPath.stringValue = currentCoursePath;
        }

        private void UpdateAvailableCourses()
        {
            trainingCourseStreamingAssetsPaths = Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, defaultCoursePath).Replace('/', Path.DirectorySeparatorChar), "*.json", SearchOption.AllDirectories)
                .Where(FileUtils.IsCourseFile)
                .Select(path => path.Substring(Application.streamingAssetsPath.Length + 1))
                .ToArray();

            // Create dummy entry if no files are present.
            if (trainingCourseStreamingAssetsPaths.Length == 0)
            {
                trainingCourseDisplayNames = new string[] { "<none>" };
                return;
            }

            trainingCourseStreamingAssetsPaths = trainingCourseStreamingAssetsPaths.OrderByAlphaNumericNaturalSort(s => s).ToArray();
            trainingCourseDisplayNames = trainingCourseStreamingAssetsPaths.Select(Path.GetFileNameWithoutExtension).ToArray();
        }
    }
}
