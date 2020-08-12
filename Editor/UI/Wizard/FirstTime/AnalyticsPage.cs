using Innoactive.CreatorEditor.Analytics;
using UnityEditor;
using UnityEngine;

namespace Innoactive.Creator.Core.Editor.UI.Wizard
{
    public class AnalyticsPage : WizardPage
    {
        [SerializeField]
        private Vector2 scrollPosition;

        private GUIStyle titleStyle = null;
        private GUIStyle textStyle = null;
        private GUIStyle urlStyle = null;

        public AnalyticsPage() : base("Step 3: Analytics")
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
            SetupStyles();

            GUILayout.BeginArea(new Rect(window.x, window.y, window.width, window.height));
                GUILayout.Label("Help Us! Contribute to Improvements!", titleStyle);
                GUILayout.Box("Innoactive Creator is actively evolving. Please, help us with your feedback. Provide us your <b>anonymous</b> usage data and contribute to improvements.", textStyle);

                GUILayout.Label("We Respect Your Privacy", titleStyle);
                GUILayout.Box("We DO NOT collect any sensitive information such as source code, file names or your courses' structure.\n\nHere is what we collect:\n- exact version of Innoactive Creator\n- exact version of Unity Editor\n- your system's language\n- information about usage of the Innoactive Creator's components\n\nIn order to collect the information above, we store a unique identifier within Unity's Editor Preferences. Your data is anonymized.", textStyle);

                GUILayout.Label("We Are Transparent", titleStyle);
                GUILayout.Box("The Innoactive Creator is open-source. Feel free to check our analytics code in <b>Core/Editor/Analytics</b>\n\nIf you want to opt-out of tracking, open <b>Innoactive > Creator > Windows > Analytics Settings</b> in the Unity's menu bar and choose <i>disabled</i> from the drop-down menu.", textStyle);

                GUILayout.Space(4f);
                if (GUILayout.Button(" > Data Privacy Information", urlStyle, GUILayout.ExpandWidth(false)))
                {
                    AnalyticsUtils.ShowDataPrivacyStatement();
                }
            GUILayout.EndArea();
        }

        private void SetupStyles()
        {
            if (titleStyle != null)
            {
                return;
            }

            titleStyle = new GUIStyle(EditorStyles.largeLabel);
            titleStyle.fontSize = 16;
            titleStyle.fontStyle = FontStyle.Bold;
            titleStyle.normal.textColor = Color.white;
            titleStyle.padding = new RectOffset(8, 4, 16, 0);

            textStyle = new GUIStyle(GUI.skin.label);
            textStyle.alignment = TextAnchor.UpperLeft;
            textStyle.fontSize = 13;
            textStyle.richText = true;
            textStyle.clipping = TextClipping.Clip;
            textStyle.wordWrap = true;
            textStyle.padding = new RectOffset(32, 4, 0, 2);

            urlStyle = new GUIStyle(EditorStyles.linkLabel);
            urlStyle.fontStyle = FontStyle.Bold;
        }
    }
}
