using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Innoactive.CreatorEditor.UI.Windows;

namespace Innoactive.CreatorEditor.UI.UIElements
{
    public class ICDelayBehavior : VisualElement, ICBehavior
    {

        public void toggleCollapse()
        {
            //add class display: none to body
            //Debug.Log(this.Children().OfType<ICDelayBehaviorBody>());
            foreach(var item in this.Children())
            {
                if(item is ICDelayBehaviorBody)
                {
                    if(item.style.display == DisplayStyle.None)
                    {
                        item.style.display = DisplayStyle.Flex;
                    }
                    else
                    {
                        item.style.display = DisplayStyle.None;
                    }
                    
                }
            }
        }

        public new class UxmlFactory : UxmlFactory<ICDelayBehavior, UxmlTraits> {
            
        }
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            
            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate = ve as ICDelayBehavior;

                ate.Clear();

                Debug.Log("DELAY BEHAVIOR ______________");
            }


        }

       
    }

}

