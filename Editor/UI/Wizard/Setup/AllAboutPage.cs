using VPG.Editor.Analytics;
using UnityEngine;

namespace VPG.Editor.UI.Wizard
{
    internal class AllAboutPage : WizardPage
    {
        public AllAboutPage() : base("Help & Documentation", false ,false )
        {

        }

        public override void Draw(Rect window)
        {
            GUILayout.BeginArea(window);
                GUILayout.Label("Hit Play to Preview", VPGEditorStyles.Title);
                GUILayout.Label("Have a look at How-To's and an in-depth Webinar for further information.", VPGEditorStyles.Paragraph);
                GUILayout.Label("How-To's", VPGEditorStyles.Header);

                VPGGUILayout.DrawLink("How to build your VR Training application", "https://developers.innoactive.de/documentation/creator/latest/articles/getting-started/designer.html", VPGEditorStyles.IndentLarge);
                VPGGUILayout.DrawLink("How to extend the Creator using a training template", "https://developers.innoactive.de/documentation/creator/latest/articles/developer/01-introduction.html", VPGEditorStyles.IndentLarge);

                GUILayout.Label("Need Help?", VPGEditorStyles.Header);

                VPGGUILayout.DrawLink("In-depth webinar on how the Creator works", "https://vimeo.com/417328541/93a752e72c", VPGEditorStyles.IndentLarge);
                VPGGUILayout.DrawLink("Visit our developer community", "https://innoactive.io/creator/community", VPGEditorStyles.IndentLarge);
                VPGGUILayout.DrawLink("Contact Us for Support", "https://www.innoactive.io/support", VPGEditorStyles.IndentLarge);

                GUILayout.Space(VPGEditorStyles.Indent);

                GUILayout.Label("Also, if you are facing any issues, don't hesitate to reach out to us for support", VPGEditorStyles.Label);
            GUILayout.EndArea();
        }
    }
}
