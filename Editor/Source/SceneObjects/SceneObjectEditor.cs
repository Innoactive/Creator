using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using Innoactive.Hub.Logging;
using Innoactive.Hub.Training.Configuration;
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
        private ISceneObject lastObject;

        private ISceneObjectRegistry SceneObjectRegistry
        {
            get { return RuntimeConfigurator.Configuration.SceneObjectRegistry; }
        }

        public void OnEnable()
        {
            ISceneObject targetObject = (ISceneObject)target;

            if (SceneObjectRegistry.ContainsGuid(targetObject.Guid))
            {
                return;
            }

            targetObject.SetSuitableName();
        }

        public void OnDisable()
        {
            lastObject = (ISceneObject)target;
        }

        public void OnDestroy()
        {
            if (target == null)
            {
                SceneObjectRegistry.Unregister(lastObject);
            }
        }

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

