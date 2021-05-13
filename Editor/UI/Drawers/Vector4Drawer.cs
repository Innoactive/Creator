using System;
using UnityEditor;
using UnityEngine;

namespace VPG.CreatorEditor.UI.Drawers
{
    /// <summary>
    /// Training drawer for `Vector4`.
    /// </summary>
    [DefaultTrainingDrawer(typeof(Vector4))]
    internal class Vector4Drawer : AbstractDrawer
    {
        /// <inheritdoc />
        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            rect.height = EditorDrawingHelper.SingleLineHeight * 2f + 2f;

            Vector4 newValue = EditorGUI.Vector4Field(rect, label, (Vector4)currentValue);

            if (newValue != (Vector4)currentValue)
            {
                ChangeValue(() => newValue, () => currentValue, changeValueCallback);
            }

            return rect;
        }
    }
}
