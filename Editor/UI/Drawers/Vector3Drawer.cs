using System;
using Innoactive.Creator.Core.Utils;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Drawers
{
    /// <summary>
    /// Default drawer for `Vector3`.
    /// </summary>
    [DefaultTrainingDrawer(typeof(Vector3))]
    public class Vector3Drawer : AbstractDrawer
    {
        /// <inheritdoc />
        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            rect.height = EditorDrawingHelper.SingleLineHeight * 2f + 2f;
            Vector3 newValue = EditorGUI.Vector3Field(rect, label, (Vector3) currentValue);

            if (newValue != (Vector3) currentValue)
            {
                ChangeValue(() => newValue, () => currentValue, changeValueCallback);
            }

            return rect;
        }
    }
}
