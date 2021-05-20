using System;
using System.IO;
using VPG.Core;
using VPG.Core.Configuration;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace VPG.Editor.Setup
{
    /// <summary>
    /// Helper class to setup scenes and trainings.
    /// </summary>
    internal class SceneSetupUtils
    {
        public const string SceneDirectory = "Assets/Scenes";
        private const string SimpleExampleName = "Hello Gizmo - A 5-step Guide";

        /// <summary>
        /// Creates and saves a new scene with given <paramref name="sceneName"/>.
        /// </summary>
        /// <param name="sceneName">Name of the scene.</param>
        /// <param name="directory">Directory to save scene in.</param>
        public static void CreateNewScene(string sceneName, string directory = SceneDirectory)
        {
            if (Directory.Exists(directory) == false)
            {
                Directory.CreateDirectory(directory);
            }
            Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            EditorSceneManager.SaveScene(newScene, $"{directory}/{sceneName}.unity");
            EditorSceneManager.OpenScene($"{directory}/{sceneName}.unity");
        }

        /// <summary>
        /// Returns true if provided <paramref name="sceneName"/> exits in given <paramref name="directory"/>.
        /// </summary>
        public static bool SceneExists(string sceneName, string directory = SceneDirectory)
        {
            return File.Exists($"{directory}/{sceneName}.unity");
        }

        /// <summary>
        /// Sets up the current scene and creates a new training course for this scene.
        /// </summary>
        /// <param name="courseName">Name of the training course.</param>
        public static void SetupSceneAndTraining(string courseName)
        {
            TrainingSceneSetup.Run();

            string errorMessage = null;
            if (CourseAssetUtils.DoesCourseAssetExist(courseName) || CourseAssetUtils.CanCreate(courseName, out errorMessage))
            {
                if (CourseAssetUtils.DoesCourseAssetExist(courseName))
                {
                     CourseAssetManager.Load(courseName);
                }
                else
                {
                    CourseAssetManager.Import(EntityFactory.CreateCourse(courseName));
                    AssetDatabase.Refresh();
                }

                SetCourseInCurrentScene(courseName);
            }

            if (string.IsNullOrEmpty(errorMessage) == false)
            {
                Debug.LogError(errorMessage);
            }

            try
            {
                EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        /// <summary>
        /// Sets the course with given <paramref name="courseName"/> for the current scene.
        /// </summary>
        /// <param name="courseName">Name of the course.</param>
        public static void SetCourseInCurrentScene(string courseName)
        {
            RuntimeConfigurator.Instance.SetSelectedCourse(CourseAssetUtils.GetCourseStreamingAssetPath(courseName));
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            GlobalEditorHandler.SetCurrentCourse(courseName);
            GlobalEditorHandler.StartEditingCourse();
        }

        /// <summary>
        /// Creates and saves a new simple example scene.
        /// </summary>
        /// <remarks>The new scene is meant to be used for step by step guides.</remarks>
        public static void CreateNewSimpleExampleScene()
        {
            string courseName = SimpleExampleName;
            int counter = 1;

            while (CourseAssetUtils.DoesCourseAssetExist(courseName) || CourseAssetUtils.CanCreate(courseName, out string errorMessage) == false)
            {
                courseName = $"{SimpleExampleName}_{counter}";
                counter++;
            }

            CreateNewScene(courseName);

            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = "Sphere";
            sphere.transform.position = new Vector3(0f, 0.5f, 2f);

            GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.name = "Plane";
            plane.transform.localScale = new Vector3(2f, 2f, 2f);

            SetupSceneAndTraining(courseName);
        }
    }
}
