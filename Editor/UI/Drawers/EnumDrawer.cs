using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Drawers
{
    /// <summary>
    /// Training drawer for `System.Enum` members.
    /// </summary>
    [DefaultTrainingDrawer(typeof(Enum))]
    public class EnumDrawer : AbstractDrawer
    {
        /// <inheritdoc />
        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            rect.height = EditorDrawingHelper.SingleLineHeight;

            Enum oldValue = (Enum)currentValue;

            Enum newValue;

            if (currentValue.GetType().GetAttributes(true).Any(atttribute => atttribute is FlagsAttribute))
            {
                newValue = EditorGUI.EnumFlagsField(rect, label, oldValue);
            }
            else
            {
                newValue = EditorGUI.EnumPopup(rect, label, oldValue);
            }

            if (newValue.Equals(oldValue))
            {
                return rect;
            }

            ChangeValue(() => newValue, () => oldValue, changeValueCallback);

            return rect;
        }
    }
}
