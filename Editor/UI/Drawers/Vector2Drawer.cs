using System;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Drawers
{
    /// <summary>
    /// Training drawer for 'Vector2'.
    /// </summary>
    [DefaultTrainingDrawer(typeof(Vector2))]
    public class Vector2Drawer : AbstractDrawer
    {
        /// <inheritdoc />
        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            rect.height = EditorDrawingHelper.SingleLineHeight * 2f + 2f;

            Vector2 newValue = EditorGUI.Vector2Field(rect, label, (Vector2)currentValue);

            if (newValue != (Vector2)currentValue)
            {
                ChangeValue(() => newValue, () => currentValue, changeValueCallback);
            }

            return rect;
        }
    }
}
