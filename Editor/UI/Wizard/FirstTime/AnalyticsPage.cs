using Innoactive.CreatorEditor.Analytics;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Wizard
{
    public class AnalyticsPage : WizardPage
    {
        [SerializeField]
        private Vector2 scrollPosition;

        public AnalyticsPage() : base("Step 3: Analytics", false, false)
        {

        }

        public override void Apply()
        {
            if (AnalyticsUtils.GetTrackingState() == AnalyticsState.Unknown)
            {
                AnalyticsUtils.SetTrackingTo(AnalyticsState.Enabled);
            }
        }

        public override void Draw(Rect window)
        {
            GUILayout.BeginArea(window);
                GUILayout.Label("Help Us! Contribute to Improvements!", CreatorEditorStyles.Title);
                GUILayout.Box("Innoactive Creator is actively evolving. Please, help us with your feedback. Provide us your <b>anonymous</b> usage data and contribute to improvements.", CreatorEditorStyles.Paragraph);

                GUILayout.Label("We Respect Your Privacy", CreatorEditorStyles.Header);
                GUILayout.Box("We DO NOT collect any sensitive information such as source code, file names or your courses' structure.\n\nHere is what we collect:\n- exact version of Innoactive Creator\n- exact version of Unity\n- your system's language\n- information about usage of the Innoactive Creator's components\n\nIn order to collect the information above, we store a unique identifier within Unity's Editor Preferences. Your data is anonymized.", CreatorEditorStyles.Paragraph);

                GUILayout.Label("We Are Transparent", CreatorEditorStyles.Header);
                GUILayout.Box("The Innoactive Creator is open-source. Feel free to check our analytics code in <b>Core/Editor/Analytics</b>\n\nIf you want to opt-out of tracking, open <b>Innoactive > Creator > Windows > Analytics Settings</b> in the Unity's menu bar and choose <i>disabled</i> from the drop-down menu.", CreatorEditorStyles.Paragraph);

                CreatorGUILayout.DrawLink("Data Privacy Information", AnalyticsUtils.ShowDataPrivacyStatement);
            GUILayout.EndArea();
        }
    }
}
