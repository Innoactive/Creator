using System;
using System.Reflection;
using Innoactive.Creator.Core.Configuration;
using Innoactive.Creator.Core.SceneObjects;
using Innoactive.Creator.Core.Properties;
using Innoactive.Creator.Core.Utils;
using Innoactive.CreatorEditor.UndoRedo;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Innoactive.CreatorEditor.UI.Drawers
{
    /// <summary>
    /// Training drawer for <see cref="UniqueNameReference"/> members.
    /// </summary>
    [DefaultTrainingDrawer(typeof(UniqueNameReference))]
    public class UniqueNameReferenceDrawer : AbstractDrawer
    {
        private bool isUndoOperation;
        private const string undoGroupName = "brotcat";

        /// <inheritdoc />
        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
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
            GameObject selectedSceneObject = GetGameObjectFromID(oldUniqueName, valueType);

            CheckForMisconfigurationIssues(selectedSceneObject, valueType, ref rect, ref guiLineRect);
            selectedSceneObject = EditorGUI.ObjectField(guiLineRect, label, selectedSceneObject, typeof(GameObject), true) as GameObject;

            string newUniqueName = GetIDFromSelectedObject(selectedSceneObject, valueType, oldUniqueName);

            if (oldUniqueName != newUniqueName)
            {

                RevertableChangesHandler.Do(
                    new TrainingCommand(
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

        private GameObject GetGameObjectFromID(string objectUniqueName, Type valueType)
        {
            if (string.IsNullOrEmpty(objectUniqueName))
            {
                return null;
            }

            if (RuntimeConfigurator.Configuration.SceneObjectRegistry.ContainsName(objectUniqueName) == false)
            {
                // If the saved unique name is not registered in the scene, perhaps is actually a GameObject's InstanceID
                GameObject gameObject = GetGameObjectFromInstanceID(objectUniqueName);

                if (gameObject == null)
                {
                    Debug.LogWarningFormat("{0} with Unique Name \"{1}\" could not be found.", valueType.Name, objectUniqueName);
                }

                return gameObject;
            }

            ISceneObject sceneObject = RuntimeConfigurator.Configuration.SceneObjectRegistry.GetByName(objectUniqueName);
            return sceneObject.GameObject;
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
            int instanceId;
            GameObject gameObject = null;

            if (int.TryParse(objectUniqueName, out instanceId))
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

            Debug.LogWarningFormat("Game Object \"{0}\" does not have a Training Scene Object component.", selectedSceneObject.name);
            return string.Empty;
        }

        private string GetUniqueNameFromTrainingProperty(GameObject selectedTrainingPropertyObject, Type valueType, string oldUniqueName)
        {
            ISceneObjectProperty trainingProperty = selectedTrainingPropertyObject.GetComponent(valueType) as ISceneObjectProperty;

            if (trainingProperty != null)
            {
                return trainingProperty.SceneObject.UniqueName;
            }

            Debug.LogWarningFormat("Scene Object \"{0}\" with Unique Name \"{1}\" does not have a {2} component.", selectedTrainingPropertyObject.name, oldUniqueName, valueType.Name);
            return string.Empty;
        }

        private void CheckForMisconfigurationIssues(GameObject selectedSceneObject, Type valueType, ref Rect originalRect, ref Rect guiLineRect)
        {
            if (selectedSceneObject != null && selectedSceneObject.GetComponent(valueType) == null)
            {
                string warning = string.Format("{0} is not configured as {1}", selectedSceneObject.name, valueType.Name);
                const string button = "Fix it";

                EditorGUI.HelpBox(guiLineRect, warning, MessageType.Error);
                guiLineRect = AddNewRectLine(ref originalRect);

                if (GUI.Button(guiLineRect, button))
                {
                    // Only relevant for Undoing a Training Property.
                    bool isAlreadySceneObject = selectedSceneObject.GetComponent<TrainingSceneObject>() != null && typeof(ISceneObjectProperty).IsAssignableFrom(valueType);
                    Component[] alreadyAttachedProperties = selectedSceneObject.GetComponents(typeof(Component));

                    RevertableChangesHandler.Do(
                        new TrainingCommand(
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
