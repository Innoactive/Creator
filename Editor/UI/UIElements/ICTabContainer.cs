using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine;
using Innoactive.CreatorEditor.UI.Windows;
using System.Collections;
using System.Collections.Generic;

namespace Innoactive.CreatorEditor.UI.UIElements
{
    public class ICTabContainer : VisualElement
    {

        public VisualElement tabContainerElement;
        public Button button { get; set; }

        private GenericMenu menu {get;set;}

        public void addButton()
        {
            Debug.Log("add button");
            menu = new GenericMenu();

            int delayMenuItem = 1;
            int disalbeMenuItem = 2;


            // Add a single menu item
            menu.AddItem(new GUIContent("Delay Behavior"), false,
                value => ChangeValueFromMenu(value),
                delayMenuItem);

            menu.AddItem(new GUIContent("Disable Object"), false,
                value => ChangeValueFromMenu(value),
                disalbeMenuItem);

            var menuPosition = new Vector2(button.layout.xMin, button.layout.height);
            menuPosition = this.LocalToWorld(menuPosition);
            var menuRect = new Rect(menuPosition, Vector2.zero);
            menu.DropDown(menuRect);
            //updateMenuPosition();


        }

        private void ChangeValueFromMenu(object menuItem)
        {
            switch((int)menuItem)
            {
                case 1:
                    VisualTreeAsset DelayBehaviorXML = (VisualTreeAsset)Resources.Load("UI/ICBehaviorDelay");
                    VisualElement DelayBehavior = DelayBehaviorXML.CloneTree();
                    this.Insert(0, DelayBehavior);
                    break;
                case 2: 
                    VisualTreeAsset DisableBehaviorXML = (VisualTreeAsset)Resources.Load("UI/ICBehaviorDisable");
                    VisualElement DisableBehavior = DisableBehaviorXML.CloneTree();
                    this.Insert(0, DisableBehavior);
                    break;
                default: Debug.Log("default case");
                    break;
            }
           // updateMenuPosition();
        }

        private void updateMenuPosition()
        {
           
            button = this.Query<Button>("add-button").First();
            // Get position of menu on top of target element.
            var menuPosition = new Vector2(button.layout.xMin, button.layout.height);
            menuPosition = this.LocalToWorld(menuPosition);
            var menuRect = new Rect(menuPosition, Vector2.zero);

            menu.DropDown(menuRect);
            Debug.Log(button);
        }

        public new class UxmlFactory : UxmlFactory<ICTabContainer, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits
        {


            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate = ve as ICTabContainer;
                ate.Clear();

                //load header into the visual tree of behavior
                var TabContainer = (VisualTreeAsset)Resources.Load("UI/ICTabContainer");
                VisualElement tabContainerElement = TabContainer.CloneTree();
                ate.Add(tabContainerElement);

                var addButton = tabContainerElement.Query<Button>("add-button").First();
                ate.button = addButton;
                addButton.clickable.clicked += () => ate.addButton();


            }
        }
    }
}
