using System;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Drawers
{
    /// <summary>
    /// Training drawer for `UnityEngine.Color`
    /// </summary>
    [DefaultTrainingDrawer(typeof(Color))]
    public class UnityColorDrawer : AbstractDrawer
    {
        /// <inheritdoc />
        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            rect.height = EditorDrawingHelper.SingleLineHeight;

            Color color = (Color)currentValue;
            Color newColor = EditorGUI.ColorField(rect, label, color);
            if (newColor != color)
            {
                ChangeValue(() => newColor, () => color, changeValueCallback);
            }

            return rect;
        }
    }
}
