using Innoactive.CreatorEditor.Analytics;
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
