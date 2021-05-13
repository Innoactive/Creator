using System;
using System.Reflection;
using System.Collections.Generic;
using VPG.Creator.Core.Configuration;
using VPG.Creator.Core.SceneObjects;
using VPG.Creator.Core.Properties;
using VPG.Creator.Core.Utils;
using VPG.CreatorEditor.UndoRedo;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VPG.CreatorEditor.UI.Drawers
{
    /// <summary>
    /// Training drawer for <see cref="UniqueNameReference"/> members.
    /// </summary>
    [DefaultTrainingDrawer(typeof(UniqueNameReference))]
    internal class UniqueNameReferenceDrawer : AbstractDrawer
    {
        private bool isUndoOperation;
        private const string undoGroupName = "brotcat";

        private readonly HashSet<string> missingUniqueNames = new HashSet<string>();

        /// <inheritdoc />
        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            if (RuntimeConfigurator.Exists == false)
            {
                return rect;
            }

            isUndoOperation = false;
            UniqueNameReference uniqueNameReference = (UniqueNameReference)currentValue;
            PropertyInfo valueProperty = currentValue.GetType().GetProperty("Value");
            Type valueType = ReflectionUtils.GetDeclaredTypeOfPropertyOrField(valueProperty);

            if (valueProperty == null)
            {
                throw new ArgumentException("Only ObjectReference<> implementations should inherit from the UniqueNameReference type.");
            }

            Rect guiLineRect = rect;
            string oldUniqueName = uniqueNameReference.UniqueName;
            GameObject selectedSceneObject = GetGameObjectFromID(oldUniqueName);

            if (selectedSceneObject == null && string.IsNullOrEmpty(oldUniqueName) == false && missingUniqueNames.Contains(oldUniqueName) == false)
            {
                missingUniqueNames.Add(oldUniqueName);
                Debug.LogError($"The Training Scene Object with the unique name '{oldUniqueName}' cannot be found!");
            }

            CheckForMisconfigurationIssues(selectedSceneObject, valueType, ref rect, ref guiLineRect);
            selectedSceneObject = EditorGUI.ObjectField(guiLineRect, label, selectedSceneObject, typeof(GameObject), true) as GameObject;

            string newUniqueName = GetIDFromSelectedObject(selectedSceneObject, valueType, oldUniqueName);

            if (oldUniqueName != newUniqueName)
            {
                RevertableChangesHandler.Do(
                    new CourseCommand(
                        () =>
                        {
                            uniqueNameReference.UniqueName = newUniqueName;
                            changeValueCallback(uniqueNameReference);
                        },
                        () =>
                        {
                            uniqueNameReference.UniqueName = oldUniqueName;
                            changeValueCallback(uniqueNameReference);
                        }),
                    isUndoOperation ? undoGroupName : string.Empty);

                if (isUndoOperation)
                {
                    RevertableChangesHandler.CollapseUndoOperations(undoGroupName);
                }
            }

            return rect;
        }

        private GameObject GetGameObjectFromID(string objectUniqueName)
        {
            if (string.IsNullOrEmpty(objectUniqueName))
            {
                return null;
            }

            // If the Runtime Configurator exists, we try to retrieve the Training Scene Object
            try
            {
                if (RuntimeConfigurator.Configuration.SceneObjectRegistry.ContainsName(objectUniqueName) == false)
                {
                    // If the saved unique name is not registered in the scene, perhaps is actually a GameObject's InstanceID
                    return GetGameObjectFromInstanceID(objectUniqueName);
                }

                ISceneObject sceneObject = RuntimeConfigurator.Configuration.SceneObjectRegistry.GetByName(objectUniqueName);
                return sceneObject.GameObject;
            }
            catch
            {
                return null;
            }
        }

        private string GetIDFromSelectedObject(GameObject selectedSceneObject, Type valueType, string oldUniqueName)
        {
            string newUniqueName = string.Empty;

            if (selectedSceneObject != null)
            {
                if (selectedSceneObject.GetComponent(valueType) != null)
                {
                    if (typeof(ISceneObject).IsAssignableFrom(valueType))
                    {
                        newUniqueName = GetUniqueNameFromSceneObject(selectedSceneObject);
                    }
                    else if (typeof(ISceneObjectProperty).IsAssignableFrom(valueType))
                    {
                        newUniqueName = GetUniqueNameFromTrainingProperty(selectedSceneObject, valueType, oldUniqueName);
                    }
                }
                else
                {
                    newUniqueName = selectedSceneObject.GetInstanceID().ToString();
                }
            }

            return newUniqueName;
        }

        private GameObject GetGameObjectFromInstanceID(string objectUniqueName)
        {
            GameObject gameObject = null;

            if (int.TryParse(objectUniqueName, out int instanceId))
            {
                gameObject = EditorUtility.InstanceIDToObject(instanceId) as GameObject;
            }

            return gameObject;
        }

        private string GetUniqueNameFromSceneObject(GameObject selectedSceneObject)
        {
            ISceneObject sceneObject = selectedSceneObject.GetComponent<TrainingSceneObject>();

            if (sceneObject != null)
            {
                return sceneObject.UniqueName;
            }

            Debug.LogWarning($"Game Object \"{selectedSceneObject.name}\" does not have a Training Scene Object component.");
            return string.Empty;
        }

        private string GetUniqueNameFromTrainingProperty(GameObject selectedTrainingPropertyObject, Type valueType, string oldUniqueName)
        {
            if (selectedTrainingPropertyObject.GetComponent(valueType) is ISceneObjectProperty trainingProperty)
            {
                return trainingProperty.SceneObject.UniqueName;
            }

            Debug.LogWarning($"Scene Object \"{selectedTrainingPropertyObject.name}\" with Unique Name \"{oldUniqueName}\" does not have a {valueType.Name} component.");
            return string.Empty;
        }

        private void CheckForMisconfigurationIssues(GameObject selectedSceneObject, Type valueType, ref Rect originalRect, ref Rect guiLineRect)
        {
            if (selectedSceneObject != null && selectedSceneObject.GetComponent(valueType) == null)
            {
                string warning = $"{selectedSceneObject.name} is not configured as {valueType.Name}";
                const string button = "Fix it";

                EditorGUI.HelpBox(guiLineRect, warning, MessageType.Error);
                guiLineRect = AddNewRectLine(ref originalRect);

                if (GUI.Button(guiLineRect, button))
                {
                    // Only relevant for Undoing a Training Property.
                    bool isAlreadySceneObject = selectedSceneObject.GetComponent<TrainingSceneObject>() != null && typeof(ISceneObjectProperty).IsAssignableFrom(valueType);
                    Component[] alreadyAttachedProperties = selectedSceneObject.GetComponents(typeof(Component));

                    RevertableChangesHandler.Do(
                        new CourseCommand(
                            ()=> SceneObjectAutomaticSetup(selectedSceneObject, valueType),
                            ()=> UndoSceneObjectAutomaticSetup(selectedSceneObject, valueType, isAlreadySceneObject, alreadyAttachedProperties)),
                        undoGroupName);
                }

                guiLineRect = AddNewRectLine(ref originalRect);
            }
        }

        private void SceneObjectAutomaticSetup(GameObject selectedSceneObject, Type valueType)
        {
            ISceneObject sceneObject = selectedSceneObject.GetComponent<TrainingSceneObject>() ?? selectedSceneObject.AddComponent<TrainingSceneObject>();

            if (RuntimeConfigurator.Configuration.SceneObjectRegistry.ContainsGuid(sceneObject.Guid) == false)
            {
                // Sets a UniqueName and then registers it.
                sceneObject.SetSuitableName();
            }

            if (typeof(ISceneObjectProperty).IsAssignableFrom(valueType))
            {
                sceneObject.AddTrainingProperty(valueType);
            }

            isUndoOperation = true;
        }

        private void UndoSceneObjectAutomaticSetup(GameObject selectedSceneObject, Type valueType, bool hadTrainingComponent, Component[] alreadyAttachedProperties)
        {
            ISceneObject sceneObject = selectedSceneObject.GetComponent<TrainingSceneObject>();

            if (typeof(ISceneObjectProperty).IsAssignableFrom(valueType))
            {
                sceneObject.RemoveTrainingProperty(valueType, true, alreadyAttachedProperties);
            }

            if (hadTrainingComponent == false)
            {
                Object.DestroyImmediate((TrainingSceneObject) sceneObject);
            }

            isUndoOperation = true;
        }

        private Rect AddNewRectLine(ref Rect currentRect)
        {
            Rect newRectLine = currentRect;
            newRectLine.height = EditorDrawingHelper.SingleLineHeight;
            newRectLine.y += currentRect.height + EditorDrawingHelper.VerticalSpacing;

            currentRect.height += EditorDrawingHelper.SingleLineHeight + EditorDrawingHelper.VerticalSpacing;
            return newRectLine;
        }
    }
}
