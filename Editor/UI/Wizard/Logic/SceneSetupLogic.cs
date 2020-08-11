using System.Collections;
using System.Collections.Generic;
using Innoactive.Creator.Core.Configuration;
using Innoactive.CreatorEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Innoactive.Creator.Core.Editor.UI.Wizard
{
    public class SceneSetupLogic
    {
        public static void CreateNewScene(string sceneName)
        {
            Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

            TrainingSceneSetup.Run();

            string errorMessage;

            if (CourseAssetUtils.CanCreate(sceneName, out errorMessage))
            {
                CourseAssetManager.Import(EntityFactory.CreateCourse(sceneName));
                RuntimeConfigurator.Instance.SetSelectedCourse(CourseAssetUtils.GetCourseStreamingAssetPath(sceneName));
                GlobalEditorHandler.SetCurrentCourse(sceneName);
                GlobalEditorHandler.StartEditingCourse();
            }

            EditorSceneManager.SaveScene(newScene, $"Assets/Scenes/{sceneName}.unity");
        }

        public static void SetupScene()
        {

        }
    }
}
