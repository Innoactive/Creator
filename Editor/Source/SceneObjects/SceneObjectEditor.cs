using UnityEditor;
using UnityEngine;
using Innoactive.Hub.Training.SceneObjects;
using Innoactive.Hub.Training.SceneObjects.Properties;

namespace Innoactive.Hub.Training.Editors.SceneObjects
{
    /// <summary>
    /// This class keeps track of TrainingSceneEntities and adds names to newly added entities.
    /// </summary>
    [CustomEditor(typeof(TrainingSceneObject))]
    public class SceneObjectEditor : Editor
    {
        [MenuItem ("CONTEXT/TrainingSceneObject/Remove Training Properties", false)]
        private static void RemoveTrainingProperties()
        {
            Component[] trainingProperties = Selection.activeGameObject.GetComponents(typeof(TrainingSceneObjectProperty));
            ISceneObject sceneObject = Selection.activeGameObject.GetComponent(typeof(ISceneObject)) as ISceneObject;

            foreach (Component trainingProperty in trainingProperties)
            {
                sceneObject.RemoveTrainingProperty(trainingProperty, true);
            }
        }

        [MenuItem("CONTEXT/TrainingSceneObject/Remove Training Properties", true)]
        private static bool ValidateRemoveTrainingProperties()
        {
            return Selection.activeGameObject.GetComponents(typeof(TrainingSceneObjectProperty)) != null;
        }
    }
}

