using System;
using System.Collections.Generic;
using System.Linq;
using Innoactive.Creator.Core.Behaviors;
using Innoactive.CreatorEditor.Configuration;
using Innoactive.CreatorEditor.ImguiTester;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Drawers
{
    /// <summary>
    /// Draws a dropdown button with all <see cref="InstantiationOption{IBehavior}"/> in the project, and creates a new instance of choosen behavior on click.
    /// </summary>
    [InstantiatorTrainingDrawer(typeof(IBehavior))]
    public class BehaviorInstantiatiorDrawer : AbstractInstantiatorDrawer<IBehavior>
    {
        /// <inheritdoc />
        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            if (EditorDrawingHelper.DrawAddButton(ref rect, "Add Behavior"))
            {
                IList<TestableEditorElements.MenuOption> options = ConvertFromConfigurationOptionsToGenericMenuOptions(EditorConfigurator.Instance.BehaviorsMenuContent.ToList(), currentValue, changeValueCallback);
                TestableEditorElements.DisplayContextMenu(options);
            }

            return rect;
        }
    }
}
