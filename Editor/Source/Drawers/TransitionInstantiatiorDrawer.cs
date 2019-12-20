using System;
using Innoactive.Hub.Training.Behaviors;
using Innoactive.Hub.Training.Editors.Utils;
using UnityEngine;

namespace Innoactive.Hub.Training.Editors.Drawers
{
    /// <summary>
    /// Draws a dropdown button with all <see cref="InstantiationOption{IBehavior}"/> in the project, and creates a new instance of choosen behavior on click.
    /// </summary>
    [InstantiatorTrainingDrawer(typeof(ITransition))]
    public class TransitionInstantiatiorDrawer : AbstractInstantiatorDrawer<IBehavior>
    {
        /// <inheritdoc />
        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            if (EditorDrawingHelper.DrawAddButton(ref rect, "Add Transition"))
            {
                ChangeValue(() => new Transition(), () => currentValue, changeValueCallback);
            }

            return rect;
        }
    }
}
