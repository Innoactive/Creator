using Innoactive.CreatorEditor.Analytics;
using UnityEditor;
using UnityEngine;

namespace Innoactive.Creator.Core.Editor.UI.Wizard
{
    public class AllAboutPage : WizardPage
    {
        public AllAboutPage() : base("Step 4: Help & Documentation")
        {

        }

        public override void Draw(Rect window)
        {
            GUILayout.BeginArea(window);
                GUILayout.Label("Hit Play to Preview", CreatorEditorStyles.Title);
                GUILayout.Label("Have a look at How-Tos and an in-depth Webinar for further information.", CreatorEditorStyles.Paragraph);
                GUILayout.Label("How-To's", CreatorEditorStyles.Header);

                CreatorLayout.DrawLink("How to build your VR Training application", "https://developers.innoactive.de/documentation/creator/latest/articles/getting-started/designer.html");
                CreatorLayout.DrawLink("How to extend the Creator using a training template", "https://developers.innoactive.de/documentation/creator/latest/articles/developer/01-introduction.html");

                GUILayout.Label("Need Help?", CreatorEditorStyles.Header);

                CreatorLayout.DrawLink("In-depth webinar on how the Creator works", () => {});
                CreatorLayout.DrawLink("Visit our developer community", "https://spectrum.chat/innoactive-creator");

                GUILayout.Space(CreatorEditorStyles.Indent);

                GUILayout.Label("Also, if you are facing any issues, don't hesitate to reach out to use for support", CreatorEditorStyles.Label);
            GUILayout.EndArea();
        }

        public override void Apply()
        {
            AnalyticsEvent finishedWizardEvent = new AnalyticsEvent()
            {
                Category = "creator",
                Action = "wizard_exited",
                Label = "all_about",
            };
            AnalyticsUtils.CreateTracker().Send(finishedWizardEvent);
        }
    }
}
