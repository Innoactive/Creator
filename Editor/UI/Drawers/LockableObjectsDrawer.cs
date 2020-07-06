using System;
using System.Linq;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Properties;
using Innoactive.Creator.Core.SceneObjects;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Drawers
{
    [DefaultTrainingDrawer(typeof(LockableObjectsCollection))]
    internal class LockableObjectsDrawer : DataOwnerDrawer
    {
        private LockableObjectsCollection lockableCollection;

        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            lockableCollection = (LockableObjectsCollection) currentValue;

            Rect currentPosition = new Rect(rect.x, rect.y, rect.width, EditorDrawingHelper.HeaderLineHeight);
            currentPosition.y += 10;

            GUI.Label(currentPosition,"Automatically unlocked objects in this step");

            for (int i = 0; i < lockableCollection.SceneObjects.Count; i++)
            {
                ISceneObject objectInScene = lockableCollection.SceneObjects[i];
                currentPosition = DrawSceneObject(currentPosition, objectInScene);
                currentPosition.y += EditorDrawingHelper.SingleLineHeight;
            }

            currentPosition.y += EditorDrawingHelper.SingleLineHeight;
            EditorGUI.LabelField(currentPosition, "To add new TrainingSceneObject, drag it in here:");
            currentPosition.y += EditorDrawingHelper.SingleLineHeight + EditorDrawingHelper.VerticalSpacing;

            TrainingSceneObject newSceneObject = (TrainingSceneObject) EditorGUI.ObjectField(currentPosition, null, typeof(TrainingSceneObject), true);
            if (newSceneObject != null)
            {
                lockableCollection.AddSceneObject(newSceneObject);
            }

            return rect;
        }

        private Rect DrawSceneObject(Rect currentPosition, ISceneObject sceneObject)
        {
            currentPosition.y += EditorDrawingHelper.SingleLineHeight + EditorDrawingHelper.VerticalSpacing;

            Rect objectFieldPosition = currentPosition;
            objectFieldPosition.width -= 24;
            GUI.enabled = false;
            EditorGUI.ObjectField(objectFieldPosition, (TrainingSceneObject) sceneObject, typeof(TrainingSceneObject), true);
            // If scene object is used by a property, dont allow removing it.
            GUI.enabled = lockableCollection.IsUsedInAutoUnlock(sceneObject) == false;
            objectFieldPosition.x = currentPosition.width - 24 + 6f;
            objectFieldPosition.width = 20;
            if (GUI.Button(objectFieldPosition,"x", new GUIStyle(GUI.skin.button) { fontStyle = FontStyle.Bold }))
            {
                lockableCollection.RemoveSceneObject(sceneObject);
            }
            GUI.enabled = true;

            foreach (LockableProperty property in sceneObject.Properties.Where(property => property is LockableProperty))
            {
                currentPosition = DrawProperty(currentPosition, property);
            }

            return currentPosition;
        }

        private Rect DrawProperty(Rect currentPosition, LockableProperty property)
        {
            currentPosition.y += EditorDrawingHelper.SingleLineHeight + EditorDrawingHelper.VerticalSpacing;
            Rect objectPosition = currentPosition;
            objectPosition.x += EditorDrawingHelper.IndentationWidth * 2f;
            objectPosition.width -= EditorDrawingHelper.IndentationWidth * 2f;

            GUI.enabled = lockableCollection.IsInAutoUnlockList(property) == false;
            bool isFlagged = GUI.enabled == false || lockableCollection.IsInManualUnlockList(property);
            if (EditorGUI.Toggle(currentPosition, isFlagged) != isFlagged)
            {
                // Inverted due to not updated with the toggle
                if (isFlagged)
                {
                    lockableCollection.Remove(property);
                }
                else
                {
                    lockableCollection.Add(property);
                }
            }
            GUI.enabled = true;
            EditorGUI.LabelField(objectPosition, property.GetType().Name);
            return currentPosition;
        }
    }
}
