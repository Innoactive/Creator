using System;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Behaviors;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Drawers
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
            if (EditorDrawingHelper.DrawAddButton(ref rect, "Add Transition"))
            {
                ChangeValue(() => EntityFactory.CreateTransition(), () => currentValue, changeValueCallback);
            }

            return rect;
        }
    }
}
