using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine;
using Innoactive.CreatorEditor.UI.Windows;

namespace Innoactive.CreatorEditor.UI.UIElements
{
    public class ICBehaviorHeader : VisualElement
    {
        public static int id_cnt = 0;

        public string headerTitle { get; set; }

        private Label headerLabel { get; set; }

        private bool collapsed { get; set; }

        private void delete()
        {
            Debug.Log("delete Behavior");
            
        }

        private void moveUp()
        {
            Debug.Log("move Behavior up");
        }

        private void moveDown()
        {
            Debug.Log("move Behavior down");
        }

        private void toggleCollapse()
        {
            
            ((ICBehavior)this.parent).toggleCollapse();
            updateToggleState();

        }

        public void updateToggleState()
        {
            this.collapsed = !this.collapsed;

            Image collapseButton = this.Query<Image>("collapseButton").First();
            if (this.collapsed)
            {
                collapseButton.image = Resources.Load("icon_collapsed_light") as Texture2D;
            }
            else
            {
                collapseButton.image = Resources.Load("icon_expanded_light") as Texture2D;
            }
            
            
        }

        public new class UxmlFactory : UxmlFactory<ICBehaviorHeader, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits
        {

            UxmlStringAttributeDescription m_title = new UxmlStringAttributeDescription { name = "header-title", defaultValue = "Behavior name"};
            
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate = ve as ICBehaviorHeader;
                ate.Clear();

                //load header into the visual tree of behavior
                var HeaderTree = (VisualTreeAsset)Resources.Load("UI/ICBehaviorHeader");
                VisualElement ht = HeaderTree.CloneTree();
                ate.Add(ht);


                ate.headerTitle = m_title.GetValueFromBag(bag, cc);
                //ate.headerTitle = title_str;
                
                
                var title = ht.Query<Label>("headertitle").First();
                title.text = ate.headerTitle;

                //make Buttons interactive
                var upButton = ate.Query<Button>("up").First();
                upButton.clickable.clicked += () => ate.moveUp();

                var downButton = ate.Query<Button>("down").First();
                downButton.clickable.clicked += () => ate.moveDown();

                var deleteButton = ate.Query<Button>("delete").First();
                deleteButton.clickable.clicked += () => ate.delete();

                var collapseButton = ate.Query<Button>("collapseButton").First();
                collapseButton.clickable.clicked += () => ate.toggleCollapse();

                var collapseButtonImage = ate.Query<Image>("collapseButton").First();
                ate.collapsed = false;
                collapseButtonImage.image = Resources.Load("icon_expanded_light") as Texture2D;
                

            }
        }
    }
}
