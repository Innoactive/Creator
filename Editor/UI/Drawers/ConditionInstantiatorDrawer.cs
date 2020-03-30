using System;
using System.Collections.Generic;
using System.Linq;
using Innoactive.Creator.Core.Conditions;
using Innoactive.CreatorEditor.Configuration;
using Innoactive.CreatorEditor.ImguiTester;
using UnityEditor;
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
            EditorGUI.BeginDisabledGroup(EditorConfigurator.Instance.AllowedMenuItemsSettings.GetConditionMenuOptions().Any() == false);
            if (EditorDrawingHelper.DrawAddButton(ref rect, "Add Condition"))
            {
                IList<TestableEditorElements.MenuOption> options = ConvertFromConfigurationOptionsToGenericMenuOptions(EditorConfigurator.Instance.ConditionsMenuContent, currentValue, changeValueCallback);
                TestableEditorElements.DisplayContextMenu(options);
            }
            EditorGUI.EndDisabledGroup();

            if (EditorConfigurator.Instance.AllowedMenuItemsSettings.GetConditionMenuOptions().Any() == false)
            {
                rect.y += rect.height + EditorDrawingHelper.VerticalSpacing;
                rect.width -= EditorDrawingHelper.IndentationWidth;
                EditorGUI.HelpBox(rect, "Your project does not contain any Conditions. Either create one or import an Innoactive Creator Component.", MessageType.Error);
                rect.height += rect.height + EditorDrawingHelper.VerticalSpacing;
            }

            return rect;
        }
    }
}
