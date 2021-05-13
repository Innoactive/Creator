using System;
using UnityEditor;
using UnityEngine;

namespace VPG.CreatorEditor.UI.Drawers
{
    /// <summary>
    /// Training drawer for `UnityEngine.Color32` member.
    /// </summary>
    [DefaultTrainingDrawer(typeof(Color32))]
    internal class UnityColor32Drawer : AbstractDrawer
    {
        /// <inheritdoc />
        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            rect.height = EditorDrawingHelper.SingleLineHeight;

            Color32 color = (Color32)currentValue;
            Color32 newColor = EditorGUI.ColorField(rect, label, color);
            if ((Color)newColor != color)
            {
                ChangeValue(() => newColor, () => color, changeValueCallback);
            }

            return rect;
        }
    }
}
