using System;
using VPG.Creator.Core;
using VPG.Creator.Core.Behaviors;
using UnityEditor;
using UnityEngine;

namespace VPG.CreatorEditor.UI.Drawers
{
    /// <summary>
    /// Draws a dropdown button with all <see cref="InstantiationOption{IBehavior}"/> in the project, and creates a new instance of choosen behavior on click.
    /// </summary>
    [InstantiatorTrainingDrawer(typeof(ITransition))]
    internal class TransitionInstantiatiorDrawer : AbstractInstantiatorDrawer<IBehavior>
    {
        /// <inheritdoc />
        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            EditorGUI.DrawRect(new Rect(0, rect.y, rect.width + 8, 1), new Color(26f / 256f, 26f / 256f, 26f / 256f));

            rect = new Rect(rect.x, rect.y - 5, rect.width, rect.height);
            if (EditorDrawingHelper.DrawAddButton(ref rect, "Add Transition"))
            {
                ChangeValue(() => EntityFactory.CreateTransition(), () => currentValue, changeValueCallback);
            }

            return rect;
        }
    }
}
