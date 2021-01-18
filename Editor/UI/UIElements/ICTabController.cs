using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine;
using Innoactive.CreatorEditor.UI.Windows;

namespace Innoactive.CreatorEditor.UI.UIElements
{
    public class ICTabController : VisualElement
    {

        

        public new class UxmlFactory : UxmlFactory<ICTabController, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits
        {

            
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate = ve as ICTabController;
                ate.Clear();
                //load header into the visual tree of behavior
                var Tab = (VisualTreeAsset)Resources.Load("UI/ICTab");
                VisualElement tabElement = Tab.CloneTree();
                ate.Add(tabElement);

                

                

            }
        }
    }
}
