using System;
using System.Collections.Generic;
using System.Linq;
using Innoactive.Creator.Core.Behaviors;
using Innoactive.CreatorEditor.Configuration;
using Innoactive.CreatorEditor.ImguiTester;
using UnityEditor;
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
            EditorGUI.BeginDisabledGroup(EditorConfigurator.Instance.AllowedMenuItemsSettings.GetBehaviorMenuOptions().Any() == false);
            if (EditorDrawingHelper.DrawAddButton(ref rect, "Add Behavior"))
            {
                IList<TestableEditorElements.MenuOption> options = ConvertFromConfigurationOptionsToGenericMenuOptions(EditorConfigurator.Instance.BehaviorsMenuContent.ToList(), currentValue, changeValueCallback);
                TestableEditorElements.DisplayContextMenu(options);
            }
            EditorGUI.EndDisabledGroup();

            if (EditorConfigurator.Instance.AllowedMenuItemsSettings.GetBehaviorMenuOptions().Any() == false)
            {
                rect.y += rect.height + EditorDrawingHelper.VerticalSpacing;
                rect.width -= EditorDrawingHelper.IndentationWidth;
                EditorGUI.HelpBox(rect, "Your project does not contain any Behaviors. Either create one or import an Innoactive Creator Component.", MessageType.Error);
                rect.height += rect.height + EditorDrawingHelper.VerticalSpacing;
            }

            return rect;
        }
    }
}
