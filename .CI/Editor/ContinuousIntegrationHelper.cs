using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Innoactive.Hub.CI
{
    /// <summary>
    /// This is a helper script used during Continuous integration tasks to prepare the SDK for being built, tested and deployed automatically
    /// </summary>
    public class ContinuousIntegrationHelper
    {
        private const string dummyScenePath = "Assets/";
        private const string dummySceneName = "DummyScene";

        /// <summary>
        /// Configures the current Unity Project in a way that it is compatible with the training sdk.
        /// </summary>
        public static void SetApiCompatibilityLevel()
        {
#if UNITY_2018_1_OR_NEWER
            // Force ProjectSettings to be saved as text and visible (required for tests later on!)
            EditorSettings.serializationMode = UnityEditor.SerializationMode.ForceText;
            // set supported API Compatibility Level for Hub SDK
            PlayerSettings.scriptingRuntimeVersion = ScriptingRuntimeVersion.Latest;
            PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Standalone, ApiCompatibilityLevel.NET_4_6);
#else
            // For Unity Versions prior to 2018:
            // make sure the API-Scripting Compatibility is set to full .NET (not subset)
            PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Standalone, ApiCompatibilityLevel.NET_2_0);
#endif
        }

        /// <summary>
        /// Configures the current Unity Project such that play mode tests are running.
        /// </summary>
        public static void EnablePlayModeTests()
        {
#if UNITY_2018_1_OR_NEWER
            const string ProjectSettingsAssetPath = "ProjectSettings/ProjectSettings.asset";
            SerializedObject projectSettingsManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath(ProjectSettingsAssetPath)[0]);
            SerializedProperty loadTestAssembly = projectSettingsManager.FindProperty ("playModeTestRunnerEnabled");

            loadTestAssembly.boolValue = true;

            projectSettingsManager.ApplyModifiedProperties();
#endif
        }

        /// <summary>
        /// Adds an empty scene at index 0 of the build settings scene list.
        /// If needed, adds also the login scene to the list.
        /// </summary>
        public static void CreateDummyHubScene()
        {
            Scene dummyScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            EditorSceneManager.SaveScene(dummyScene, dummyScenePath + dummySceneName + ".unity");
            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            scenes.Insert(0, new EditorBuildSettingsScene(dummyScene.path, true));
            EditorBuildSettings.scenes = scenes.ToArray();
        }
    }
}

