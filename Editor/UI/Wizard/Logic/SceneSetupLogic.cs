using Innoactive.Creator.Core.Configuration;
using Innoactive.CreatorEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Innoactive.Creator.Core.Editor.UI.Wizard
{
    /// <summary>
    /// Helper class to setup scenes and trainings.
    /// </summary>
    public class SceneSetupLogic
    {
        /// <summary>
        /// Creates and saves a new scene with given <paramref name="sceneName"/>.
        /// </summary>
        /// <param name="sceneName">Name of the scene.</param>
        /// <param name="directory">Directory to save scene in.</param>
        public static void CreateNewScene(string sceneName, string directory = "Assets/Scenes")
        {
            Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            EditorSceneManager.SaveScene(newScene, $"{directory}/{sceneName}.unity");
            EditorSceneManager.OpenScene($"{directory}/{sceneName}.unity");
        }

        /// <summary>
        /// Sets up the current scene and creates a new training course for this scene.
        /// </summary>
        /// <param name="courseName">Name of the training course.</param>
        public static void SetupSceneAndTraining(string courseName)
        {
            TrainingSceneSetup.Run();

            if (CourseAssetUtils.CanCreate(courseName, out string errorMessage))
            {
                CourseAssetManager.Import(EntityFactory.CreateCourse(courseName));
                RuntimeConfigurator.Instance.SetSelectedCourse(CourseAssetUtils.GetCourseStreamingAssetPath(courseName));
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                GlobalEditorHandler.SetCurrentCourse(courseName);
                GlobalEditorHandler.StartEditingCourse();
            }

            if (string.IsNullOrEmpty(errorMessage) == false)
            {
                Debug.LogError(errorMessage);
            }

            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        }
    }
}
