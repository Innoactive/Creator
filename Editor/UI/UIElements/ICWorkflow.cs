using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Innoactive.CreatorEditor.UI.Windows;

namespace Innoactive.CreatorEditor.UI.UIElements
{
    public class ICWorkflow : VisualElement
    {

        private GenericMenu menu { get; set; }

        private Vector2 menuPosition { get; set; }
        private const int newStep = 1;
        private const int playAtStep = 2;

        public bool dragging = false;
        public ICWorkflowStep DraggedStep;

        public ICWorkflowTransition wt;

        List<VisualElement> allSteps = new List<VisualElement>();
        

        public void HandleMouseLeave(MouseLeaveEvent evt)
        {
           
        }

        private void ChangeValueFromMenu(object menuItem)
        {
            switch ((int)menuItem)
            {
                case newStep:
                    VisualTreeAsset workflowStepXML = (VisualTreeAsset)Resources.Load("UI/ICWorkflowStep");
                    VisualElement workflowStep = workflowStepXML.CloneTree();
                    var stepContainer = this.Query<VisualElement>("stepContainer").First();
                    stepContainer.Insert(0, workflowStep);
                    workflowStep.style.top = this.menuPosition.y;
                    workflowStep.style.left = this.menuPosition.x;
                    allSteps.Add(workflowStep);
                    break;
                case playAtStep:
                    break;
                default:
                    Debug.Log("default case");
                    break;
            }
            // updateMenuPosition();
        }

        public void DrawLine(Vector2 mp)
        {
            Debug.Log("create line " + mp);
            VisualTreeAsset workflowTransitionXML = (VisualTreeAsset)Resources.Load("UI/ICWorkflowTransition");
            VisualElement workflowTransition = workflowTransitionXML.CloneTree();
            var transitionContainer = this.Query<VisualElement>("transitionsContainer").First();
            Debug.Log(transitionContainer);
          

            transitionContainer.Insert(0, workflowTransition);
            wt = (ICWorkflowTransition)workflowTransition;
            ((ICWorkflowTransition)workflowTransition).positionA = mp;
            ((ICWorkflowTransition)workflowTransition).MarkDirtyRepaint();
        }

        public void HandleMouseDown(MouseDownEvent evt)
        {

            if (evt.button != (int)MouseButton.RightMouse) return;
               Debug.Log("WORKFLOW MOUSE DOWN");
            
            
            menu = new GenericMenu();





            // Add a single menu item
            menu.AddItem(new GUIContent("create Step"), false,
                value => ChangeValueFromMenu(value),
                newStep);

            menu.AddItem(new GUIContent("play from this step"), false,
                value => ChangeValueFromMenu(value),
                playAtStep);

            this.menuPosition = new Vector2(evt.localMousePosition.x, evt.localMousePosition.y);
            var mPosition = this.LocalToWorld(this.menuPosition);
            var menuRect = new Rect(mPosition, Vector2.zero);
            menu.DropDown(menuRect);



        }

       

        public void HandleMouseMove(MouseMoveEvent evt)
        {
           
            if (dragging && DraggedStep != null)
            {

                DraggedStep.style.left = evt.mousePosition.x;
                DraggedStep.style.top = evt.mousePosition.y;
                //evt.currentTarget.style.left = evt.mousePosition.x;
                //evt.currentTarget.style.top = evt.mousePosition.y;

            }
           
        }

        public void HandleMouseUp(MouseUpEvent evt)
        {
            dragging = false;
            DraggedStep = null;
        }

        public new class UxmlFactory : UxmlFactory<ICWorkflow, UxmlTraits>
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
                var ate = ve as ICWorkflow;
                ate.Clear();

                //load header into the visual tree of behavior
                var wfTree = (VisualTreeAsset)Resources.Load("UI/ICWorkflow");
                VisualElement workflow = wfTree.CloneTree();
                ate.Add(workflow);



                Debug.Log("New workflow  ______________");

                var stepContainer = ate.Query<VisualElement>("stepContainer").First();
                stepContainer.RegisterCallback<MouseDownEvent>(evt => ate.HandleMouseDown(evt));
                stepContainer.RegisterCallback<MouseUpEvent>(evt => ate.HandleMouseUp(evt));
                stepContainer.RegisterCallback<MouseMoveEvent>(evt => ate.HandleMouseMove(evt));


            }


        }


    }

}
