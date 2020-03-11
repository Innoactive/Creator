using System;
using Innoactive.Hub.Training.Editors.Utils;
using UnityEditor;
using UnityEngine;

namespace Innoactive.Hub.Training.Editors.Drawers
{
    /// <summary>
    /// Training drawer for int values.
    /// </summary>
    [DefaultTrainingDrawer(typeof(int))]
    public class IntDrawer : AbstractDrawer
    {
        /// <inheritdoc />
        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            rect.height = EditorDrawingHelper.SingleLineHeight;

            int value = (int)currentValue;
            int newValue = EditorGUI.IntField(rect, label, value);

            if (value != newValue)
            {
                ChangeValue(() => newValue, () => value, changeValueCallback);
            }

            return rect;
        }
    }
}