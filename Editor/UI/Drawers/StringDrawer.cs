using System;
using UnityEditor;
using UnityEngine;

namespace VPG.Editor.UI.Drawers
{
    /// <summary>
    /// Training drawer for string members.
    /// </summary>
    [DefaultTrainingDrawer(typeof(string))]
    internal class StringDrawer : AbstractDrawer
    {
        /// <inheritdoc />
        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            rect.height = EditorDrawingHelper.SingleLineHeight;

            string stringValue = (string)currentValue;
            string newValue = EditorGUI.TextField(rect, label, stringValue);

            if (stringValue != newValue)
            {
                ChangeValue(() => newValue, () => stringValue, changeValueCallback);
            }

            return rect;
        }
    }
}
