using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Innoactive.CreatorEditor.UI.Windows;

namespace Innoactive.CreatorEditor.UI.UIElements
{
    public class ICDelayBehaviorBody : VisualElement
    {


        void DurationValueChanged(string duration)
        {
            Debug.Log("new Value " + duration);
        }

        public new class UxmlFactory : UxmlFactory<ICDelayBehaviorBody, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            //add a list of Bindable values
            //UxmlIntAttributeDescription m_duration = new UxmlIntAttributeDescription { name = "duration", defaultValue = 0 };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate = ve as ICDelayBehaviorBody;
                ate.Clear();

                //load body into the visual tree of behavior
                var BodyTree = (VisualTreeAsset)Resources.Load("UI/ICBehaviorDelayBody");
                VisualElement bt = BodyTree.CloneTree();
                ate.Add(bt);

                var delayTextField = ate.Query<TextField>("delayvalue").First();
                delayTextField.RegisterValueChangedCallback(x => ate.DurationValueChanged(x.newValue));
            }



        }

        public int delay_duration { get; set; }
    }
}
