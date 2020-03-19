using System;
using Innoactive.Creator.Core.Utils;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Drawers
{
    /// <summary>
    /// Training drawer for `UnityEngine.Color32` member.
    /// </summary>
    [DefaultTrainingDrawer(typeof(Color32))]
    public class UnityColor32Drawer : AbstractDrawer
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
