using System;
using System.Collections.Generic;
using Innoactive.Hub.Training.Conditions;
using Innoactive.Hub.Training.Editors.Configuration;
using Innoactive.Hub.Training.Editors.Utils;
using Innoactive.Hub.Unity.Tests.Training.Editor.EditorImguiTester;
using UnityEngine;

namespace Innoactive.Hub.Training.Editors.Drawers
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
