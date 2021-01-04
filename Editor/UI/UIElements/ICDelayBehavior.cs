using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Innoactive.CreatorEditor.UI.Windows;

namespace Innoactive.CreatorEditor.UI.UIElements
{
    public class ICDelayBehavior : VisualElement
    {
       
        private Button collapseArrowButton;
        private TemplateContainer bodyContainer;

        private TemplateContainer iconContainer;
        private Button down;
        private Button up;
        private Button delete;

        private static string behavior_name = "delay behavior";


        public ICDelayBehavior()
        {
            Debug.Log("BehaviorConstructor");
             
        }


        public void draw()
        {
           
           
            
            

            bodyContainer = new TemplateContainer();
            bodyContainer.AddToClassList("delay-behavior-body");
            Add(bodyContainer);
            var label = new Label() { text = "Delay in seconds" };
            bodyContainer.Add(label);
            var textField = new TextField() { value= "0"};
            
            bodyContainer.Add(textField);

        }

        void ToggleCollapseBehavior( string s)
        {
            
            //Debug.Log(bodyContainer.ClassList);
            //if(bodyContainer.ClassList)
            Texture2D iconAsset;
            if(bodyContainer.style.display == DisplayStyle.None)
            {
                bodyContainer.style.display = DisplayStyle.Flex;
                iconAsset = Resources.Load<Texture2D>("icon_expanded_light");

            }
            else
            {

                bodyContainer.style.display = DisplayStyle.None;
                iconAsset = Resources.Load<Texture2D>("icon_collapsed_light");
            }
            collapseArrowButton.style.backgroundImage = iconAsset;






        }

        void SelectHeader()
        {
        }

        void DurationValueChanged(string duration)
        {
            Debug.Log("new Value " + duration);
        }

        public new class UxmlFactory : UxmlFactory<ICDelayBehavior, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            //add a list of Bindable values
            UxmlIntAttributeDescription m_duration = new UxmlIntAttributeDescription { name = "duration", defaultValue = 0 };
          
            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate = ve as ICDelayBehavior;

                ate.Clear();

                //load body into the visual tree of behavior
                var BodyTree = (VisualTreeAsset)Resources.Load("UI/ICBehaviorDelayBody");
                VisualElement bt = BodyTree.CloneTree();
                StepWindow.root.Add(bt);


                var delayTextField = StepWindow.root.Query<TextField>("delayvalue").First();
                delayTextField.RegisterValueChangedCallback(x => ate.DurationValueChanged(x.newValue));
            }


        }

        public int delay_duration { get; set; }
    }

}

