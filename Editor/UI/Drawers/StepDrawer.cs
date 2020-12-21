using System;
using Innoactive.Creator.Core;
using Innoactive.CreatorEditor.Configuration;
using Innoactive.CreatorEditor.Tabs;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Innoactive.CreatorEditor.UI.Windows;


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
        private LockablePropertyTab lockablePropertyTab;

        class UD
        {
            string label = "myTest";

            public string getLabel()
            {
                Debug.Log("returning label");
                return label;
            }
        }

        protected StepDrawer()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

            var visualTree = (VisualTreeAsset)Resources.Load("UI/ICFormRow");
            
            VisualElement stepdrawerRoot = visualTree.CloneTree();
            stepdrawerRoot.ToggleInClassList("form-element");


            UD ud = new UD();
            
            stepdrawerRoot.userData = (UD) ud;
            //Debug.Log((UD)(stepdrawerRoot.userData).getLabel());
            StepWindow.root.Add(stepdrawerRoot);
            StyleSheet style = (StyleSheet)Resources.Load("UI/ICFormRowStyle");
            StepWindow.root.styleSheets.Add(style);

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

            if (lastStep != step)
            {
                lockablePropertyTab = new LockablePropertyTab(new GUIContent("Unlocked Objects"), step);
                lastStep = step;
            }

            GUIContent behaviorLabel = new GUIContent("Behaviors");
            if (EditorConfigurator.Instance.Validation.LastReport != null && EditorConfigurator.Instance.Validation.LastReport.GetBehaviorEntriesFor(step).Count > 0)
            {
                behaviorLabel.image = EditorGUIUtility.IconContent("Warning").image;
            }

            GUIContent transitionLabel = new GUIContent("Transitions");
            if (EditorConfigurator.Instance.Validation.LastReport != null && EditorConfigurator.Instance.Validation.LastReport.GetConditionEntriesFor(step).Count > 0)
            {
                transitionLabel.image = EditorGUIUtility.IconContent("Warning").image;
            }
                
            TabsGroup activeTab = new TabsGroup(
                step.Metadata,
                new DynamicTab(behaviorLabel, () => step.Behaviors, value => step.Behaviors = (IBehaviorCollection)value),
                new DynamicTab(transitionLabel, () => step.Transitions, value => step.Transitions = (ITransitionCollection)value),
                lockablePropertyTab
            ); 
             
            Rect tabRect = new TabsGroupDrawer().Draw(new Rect(rect.x, rect.y + rect.height + 4f, rect.width, 0), activeTab, changeValueCallback, label);
            rect.height += tabRect.height;
            return rect;
        }
        
        private void myButtonDraw(Button button)
        {
            var b = button.Q(className: "default-button");
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


            //VisualElement stepname = new Label("Hello world");
            //StepWindow.root.Add(stepname);

            var formLabel = StepWindow.root.Q<Label>("form-label");
            var formTextfield = StepWindow.root.Q<TextField>("form-textfield");
            //formLabel.text = "StepName";
            //Debug.Log(formTextfield.value);



            //rect.height = labelStyle.CalcHeight(new GUIContent("Step Name"), rect.width);

            //EditorGUI.LabelField(typeRect, "Step Name", labelStyle);

            string oldName = step.Name;
            string newName = formTextfield.value;//EditorGUI.DelayedTextField(nameRect, step.Name, textFieldStyle);

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
            
        }
    }
}
