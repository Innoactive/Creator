using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine;
using Innoactive.CreatorEditor.UI.Windows;

namespace Innoactive.CreatorEditor.UI.UIElements
{
    public class ICTabHeader : VisualElement
    {



        public new class UxmlFactory : UxmlFactory<ICTabHeader, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits
        {


            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate = ve as ICTabHeader;

                //load header into the visual tree of behavior
                var TabHeader = (VisualTreeAsset)Resources.Load("UI/ICTabHeader");
                VisualElement tabHeaderElement = TabHeader.CloneTree();
                StepWindow.root.Add(tabHeaderElement);

                ate.Clear();




            }
        }
    }
}
