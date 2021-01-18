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
