using System;
using Innoactive.Creator.Core.Utils;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Drawers
{
    /// <summary>
    /// Training drawer for float members.
    /// </summary>
    [DefaultTrainingDrawer(typeof(float))]
    public class FloatDrawer : AbstractDrawer
    {
        /// <inheritdoc />
        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            rect.height = EditorDrawingHelper.SingleLineHeight;

            float value = (float)currentValue;
            float newValue = EditorGUI.DelayedFloatField(rect, label, value);

            // Rounding error can't take place here.
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (value != newValue)
            {
                ChangeValue(() => newValue, () => value, changeValueCallback);
            }

            return rect;
        }
    }
}
