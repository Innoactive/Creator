using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Innoactive.CreatorEditor.UI.Windows;

namespace Innoactive.CreatorEditor.UI.UIElements
{
    public class ICWorkflowStep : VisualElement
    {

        private bool startDragging = false;

       

        public void HandleMouseLeave(MouseLeaveEvent evt)
        {
            startDragging = false;
        }

        public void CreateLine(MouseDownEvent evt)
        {



            ((ICWorkflow)this.parent.parent.parent.parent).DrawLine(evt.mousePosition);
        }

            public void HandleMouseDown(MouseDownEvent evt)
        {
            Debug.Log("WORKFLOW STEP MOUSE DOWN");
            startDragging = true;
            Debug.Log(this.parent.parent.parent.parent);
            ((ICWorkflow)this.parent.parent.parent.parent).DraggedStep = this;
            ((ICWorkflow)this.parent.parent.parent.parent).dragging = true;

        }

        public void HandleMouseMove(MouseMoveEvent evt)
        {
            
            if (startDragging)
            {
                Debug.Log(evt.mousePosition);
                var stepElement = this.Query<VisualElement>("step").First();
                stepElement.style.left = evt.mousePosition.x;
                stepElement.style.top = evt.mousePosition.y;
                

            }
        }

        public void MoveTo(Vector2 position)
        {
            var stepElement = this.Query<VisualElement>("step").First();
            stepElement.style.left = position.x;
            stepElement.style.top = position.y;
        }

        public void HandleMouseUp(MouseUpEvent evt)
        {
            Debug.Log(evt);
            startDragging = false;


        }

        public new class UxmlFactory : UxmlFactory<ICWorkflowStep, UxmlTraits>
        {

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
                var ate = ve as ICWorkflowStep;
                ate.Clear();

                //load header into the visual tree of behavior
                var stepTree = (VisualTreeAsset)Resources.Load("UI/ICWorkflowStepContent");
                VisualElement step = stepTree.CloneTree();
                ate.Add(step);


                Debug.Log("New Step ______________");

                var stepElement = ate.Query<VisualElement>("step").First();
                //stepElement.RegisterCallback<MouseLeaveEvent>(evt => ate.HandleMouseLeave(evt));
                stepElement.RegisterCallback<MouseDownEvent>(evt => ate.HandleMouseDown(evt));
                //stepElement.RegisterCallback<MouseMoveEvent>(evt => ate.HandleMouseMove(evt));
                //stepElement.RegisterCallback<MouseUpEvent>(evt => ate.HandleMouseUp(evt));

                //ICWorkflowStep.parent.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
                var exit = ate.Query<VisualElement>("exit").First();
                exit.RegisterCallback<MouseDownEvent>(evt => ate.CreateLine(evt));
            }


        }


    }

}
