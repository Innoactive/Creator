using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Wizard
{
    public class AllAboutPage : WizardPage
    {
        public AllAboutPage() : base("Step 4: Help & Documentation", false ,false )
        {

        }

        public override void Draw(Rect window)
        {
            GUILayout.BeginArea(window);
            GUILayout.Label("Hit Play to Preview", CreatorEditorStyles.Title);
                GUILayout.Label("Have a look at How-To's and an in-depth Webinar for further information.", CreatorEditorStyles.Paragraph);
                GUILayout.Label("How-To's", CreatorEditorStyles.Header);

                CreatorGUILayout.DrawLink("How to build your VR Training application", "https://developers.innoactive.de/documentation/creator/latest/articles/getting-started/designer.html", CreatorEditorStyles.IndentLarge);
                CreatorGUILayout.DrawLink("How to extend the Creator using a training template", "https://developers.innoactive.de/documentation/creator/latest/articles/developer/01-introduction.html", CreatorEditorStyles.IndentLarge);

                GUILayout.Label("Need Help?", CreatorEditorStyles.Header);

                CreatorGUILayout.DrawLink("In-depth webinar on how the Creator works", "https://vimeo.com/417328541/93a752e72c", CreatorEditorStyles.IndentLarge);
                CreatorGUILayout.DrawLink("Visit our developer community", "https://spectrum.chat/innoactive-creator", CreatorEditorStyles.IndentLarge);
                CreatorGUILayout.DrawLink("Contact Us for Support", "https://www.innoactive.io/support", CreatorEditorStyles.IndentLarge);

                GUILayout.Space(CreatorEditorStyles.Indent);

                GUILayout.Label("Also, if you are facing any issues, don't hesitate to reach out to us for support", CreatorEditorStyles.Label);
            GUILayout.EndArea();
        }
    }
}
