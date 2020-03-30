using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Drawers
{
    /// <summary>
    /// Custom color drawer for the case when color is stored as a list of four floats.
    /// </summary>
    public class ListFloatColorDrawer : AbstractDrawer
    {
        private static List<float> ColorToList(Color color)
        {
            return new List<float>
            {
                color.r,
                color.g,
                color.b,
                color.a
            };
        }

        /// <inheritdoc />
        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            rect.height = EditorDrawingHelper.SingleLineHeight;

            List<float> list = (List<float>)currentValue;
            if (list == null)
            {
                list = ColorToList(Color.white);
            }

            Color oldColor = new Color(list[0], list[1], list[2], list[3]);
            Color newColor = EditorGUI.ColorField(rect, label, oldColor);

            if (newColor == oldColor)
            {
                return rect;
            }

            ChangeValue(() => ColorToList(newColor), () => ColorToList(oldColor), changeValueCallback);

            return rect;
        }
    }
}
