using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.UIElements
{

    public class Behavior : VisualElement
    {
       

        private Button collapseArrowButton;
        private TemplateContainer bodyContainer;

        private TemplateContainer iconContainer;
        private Button down;
        private Button up;
        private Button delete;

        public Behavior()
        {
            
        }


        public void draw()
        {
           

            var headerContainer = new TemplateContainer();
            headerContainer.AddToClassList("delay-behavior-header");
            Add(headerContainer);

            var ArrowAndLabelContainer = new TemplateContainer();
            ArrowAndLabelContainer.AddToClassList("arrow-and-label");
            headerContainer.Add(ArrowAndLabelContainer);
            collapseArrowButton = new Button();
            collapseArrowButton.clickable.clicked += () => ToggleCollapseBehavior(collapseArrowButton.parent.name);

            var iconAsset = Resources.Load<Texture2D>("icon_expanded_light");
           // EditorIcon ingoingIcon = new EditorIcon("icon_arrow_right");
            //var iconAsset= ingoingIcon.Texture;
            collapseArrowButton.style.backgroundImage = iconAsset;
            ArrowAndLabelContainer.Add(collapseArrowButton);

            var header = new Label() { text = "DelayBehavior" };
            ArrowAndLabelContainer.Add(header);
            ArrowAndLabelContainer.AddManipulator(new Clickable(() => SelectHeader()));

            iconContainer = new TemplateContainer();
            iconContainer.AddToClassList("micons");
            down = new Button();
            var downAsset = Resources.Load<Texture2D>("icon_arrow_down_light");
            down.style.backgroundImage = downAsset;


            up = new Button();
            var upAsset = Resources.Load<Texture2D>("icon_arrow_up_light");
            up.style.backgroundImage = upAsset;


            delete = new Button();
            var delAsset = Resources.Load<Texture2D>("icon_delete_light");
            delete.style.backgroundImage = delAsset;

            iconContainer.Add(up);
            iconContainer.Add(down);
            iconContainer.Add(delete);
           

            headerContainer.Add(iconContainer);
            

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

        public new class UxmlFactory : UxmlFactory<Behavior, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            
            
        }
    }

}

