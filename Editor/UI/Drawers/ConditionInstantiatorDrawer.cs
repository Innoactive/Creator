using System;
using System.Collections.Generic;
using Innoactive.Creator.Core.Conditions;
using Innoactive.Creator.Core.Utils;
using Innoactive.CreatorEditor.Configuration;
using Innoactive.CreatorEditor.ImguiTester;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Drawers
{
    /// <summary>
    /// Draws a dropdown button with all <see cref="InstantiationOption{ICondition}"/> in the project, and creates a new instance of choosen condition on click.
    /// </summary>
    [InstantiatorTrainingDrawer(typeof(ICondition))]
    public class ConditionInstantiatorDrawer : AbstractInstantiatorDrawer<ICondition>
    {
        /// <inheritdoc />
        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            if (EditorDrawingHelper.DrawAddButton(ref rect, "Add Condition"))
            {
                IList<TestableEditorElements.MenuOption> options = ConvertFromConfigurationOptionsToGenericMenuOptions(EditorConfigurator.Instance.ConditionsMenuContent, currentValue, changeValueCallback);
                TestableEditorElements.DisplayContextMenu(options);
            }

            return rect;
        }
    }
}
