using System;
using Innoactive.CreatorEditor.Analytics;
using Innoactive.CreatorEditor.UI;
using UnityEditor;
using UnityEngine;

internal class AnalyticsSettingsProvider : SettingsProvider
{
    const string Path = "Project/Creator/Analytics";

    public AnalyticsSettingsProvider() : base(Path, SettingsScope.Project) {}

    public override void OnGUI(string searchContext)
    {
        GUILayout.Label("Help Us! Contribute to Improvements!", CreatorEditorStyles.Title);
        GUILayout.Box("Innoactive Creator is actively evolving. Please, help us with your feedback. Provide us your <b>anonymous</b> usage data and contribute to improvements.", CreatorEditorStyles.Paragraph);

        GUILayout.Label("We Respect Your Privacy", CreatorEditorStyles.Header);
        GUILayout.Box("We DO NOT collect any sensitive information such as source code, file names or your courses' structure.\n\nHere is what we collect:\n- exact version of Innoactive Creator\n- exact version of Unity\n- your system's language\n- information about usage of the Innoactive Creator's components\n\nIn order to collect the information above, we store a unique identifier within Unity's Editor Preferences. Your data is anonymized.", CreatorEditorStyles.Paragraph);

        GUILayout.Label("We Are Transparent", CreatorEditorStyles.Header);
        GUILayout.Box("The Innoactive Creator is open-source. Feel free to check our analytics code in <b>Core/Editor/Analytics</b>\n\nIf you want to opt-out of tracking choose <i>disabled</i> from the drop-down menu.", CreatorEditorStyles.Paragraph);

        CreatorGUILayout.DrawLink("Data Privacy Information", AnalyticsUtils.ShowDataPrivacyStatement);

        GUILayout.Space(16);

        // Unknown should not be shown, so we have to remove it.
        int state = (int)AnalyticsUtils.GetTrackingState() - 1;
        string[] labels = {"Disabled", "Enabled"};

        int newState = EditorGUILayout.Popup("Analytics Tracking", state, labels, CreatorEditorStyles.Popup);
        if (newState != state)
        {
            AnalyticsUtils.SetTrackingTo((AnalyticsState)Enum.ToObject(typeof(AnalyticsState), newState + 1));
        }
    }

    [SettingsProvider]
    public static SettingsProvider Provider()
    {
        SettingsProvider provider = new AnalyticsSettingsProvider();
        return provider;
    }
}
