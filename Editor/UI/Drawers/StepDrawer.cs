using System;
using Innoactive.Creator.Core;
using Innoactive.CreatorEditor.Tabs;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Drawers
{
    /// <summary>
    /// Drawer for a step to skip NameableDrawer.
    /// Skip label draw call, as well.
    /// </summary>
    [DefaultTrainingDrawer(typeof(Step.EntityData))]
    internal class StepDrawer : ObjectDrawer
    {
        private IStepData lastStep;
        private TabsGroup activeTab;

        protected StepDrawer()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        ~StepDrawer()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            rect = base.Draw(rect, currentValue, changeValueCallback, label);

            Step.EntityData step = (Step.EntityData) currentValue;

            if (step.Metadata == null)
            {
                step.Metadata = new Metadata();
            }

            if (activeTab == null || lastStep != step)
            {
                GUIContent behaviorLabel = new GUIContent("Behaviors");
                GUIContent transitionLabel = new GUIContent("Transitions");

                activeTab = new TabsGroup(
                    step.Metadata,
                    new DynamicTab(behaviorLabel, () => step.Behaviors, value => step.Behaviors = (IBehaviorCollection)value),
                    new DynamicTab(transitionLabel, () => step.Transitions, value => step.Transitions = (ITransitionCollection)value),
                    new LockablePropertyTab(new GUIContent("Unlocked Objects"), step)
                );
                lastStep = step;
            }

            Rect tabRect = new TabsGroupDrawer().Draw(new Rect(rect.x, rect.y + rect.height + 4f, rect.width, 0), activeTab, changeValueCallback, label);
            rect.height += tabRect.height;
            return rect;
        }

        protected override float DrawLabel(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            Step.EntityData step = currentValue as Step.EntityData;

            Rect nameRect = rect;
            nameRect.width = EditorGUIUtility.labelWidth;
            Rect typeRect = rect;
            typeRect.x += EditorGUIUtility.labelWidth;
            typeRect.width -= EditorGUIUtility.labelWidth;

            GUIStyle textFieldStyle = new GUIStyle(EditorStyles.textField)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 12
            };

            GUIStyle labelStyle = new GUIStyle(EditorStyles.label)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 12
            };

            rect.height = labelStyle.CalcHeight(new GUIContent("Step Name"), rect.width);

            EditorGUI.LabelField(typeRect, "Step Name", labelStyle);

            string oldName = step.Name;
            string newName = EditorGUI.DelayedTextField(nameRect, step.Name, textFieldStyle);

            if (newName != step.Name)
            {
                ChangeValue(() =>
                    {
                        step.Name = newName;
                        return step;
                    },
                    () =>
                    {
                        step.Name = oldName;
                        return step;
                    },
                    changeValueCallback);
            }

            return rect.height;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange mode)
        {
            activeTab = null;
        }
    }
}
