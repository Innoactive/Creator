using System;
using Innoactive.Creator.Core.Behaviors;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Drawers
{
    /// <summary>
    /// Training drawer for <see cref="BehaviorExecutionStages"/> members.
    /// </summary>
    [DefaultTrainingDrawer(typeof(BehaviorExecutionStages))]
    internal class BehaviorExecutionStagesDrawer : AbstractDrawer
    {
        private enum ExecutionStages
        {
            BeforeStepExecution = 1 << 0,
            AfterStepExecution = 1 << 1,
            BeforeAndAfterStepExecution = ~0
        }

        /// <inheritdoc />
        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            rect.height = EditorDrawingHelper.SingleLineHeight;

            BehaviorExecutionStages oldBehaviorExecutionStages = (BehaviorExecutionStages)currentValue;
            BehaviorExecutionStages newBehaviorExecutionStages;
            ExecutionStages oldExecutionStages;
            ExecutionStages newExecutionStages;

            oldExecutionStages = (ExecutionStages)(int)currentValue;
            newExecutionStages = (ExecutionStages)EditorGUI.EnumPopup(rect, label, oldExecutionStages);

            if (newExecutionStages != oldExecutionStages)
            {
                switch (newExecutionStages)
                {
                    case ExecutionStages.AfterStepExecution:
                        newBehaviorExecutionStages = BehaviorExecutionStages.Deactivation;
                        break;
                    case ExecutionStages.BeforeAndAfterStepExecution:
                        newBehaviorExecutionStages = BehaviorExecutionStages.ActivationAndDeactivation;
                        break;
                    default:
                        newBehaviorExecutionStages = BehaviorExecutionStages.Activation;
                        break;
                }

                ChangeValue(() => newBehaviorExecutionStages, () => oldBehaviorExecutionStages, changeValueCallback);
            }

            return rect;
        }
    }
}
