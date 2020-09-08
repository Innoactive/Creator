using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.Analytics
{
    internal class TrackingSettingsWindow : EditorWindow
    {
        private const string menuPath = "Innoactive/Settings/Analytics Settings";

        private static TrackingSettingsWindow window;

        [MenuItem(menuPath, false, 12)]
        private static void ShowWizard()
        {
            if (window == null)
            {
                TrackingSettingsWindow[] openSettingWindows = Resources.FindObjectsOfTypeAll<TrackingSettingsWindow>();

                if (openSettingWindows.Length > 1)
                {
                    for (int i = 1; i < openSettingWindows.Length; i++)
                    {
                        openSettingWindows[i].Close();
                    }
                    Debug.LogWarning("There were more than one creator setting windows open. This should not happen. The redundant windows were closed.");
                }

                window = openSettingWindows.Length > 0 ? openSettingWindows[0] : GetWindow<TrackingSettingsWindow>();
            }

            window.Show();
            window.minSize = new Vector2(280f, 50f);
            window.maxSize = new Vector2(280f, 50f);
            window.Focus();
        }


        private void OnGUI()
        {
            titleContent = new GUIContent("Analytics Settings");

            GUILayout.Space(8);

            // Unknown should not be shown, so we have to remove it.
            int state = (int)AnalyticsUtils.GetTrackingState() - 1;
            string[] labels = {"Disabled", "Minimum", "Enabled"};

            int newState = EditorGUILayout.Popup("Analytics Tracking", state, labels);
            if (newState != state)
            {
                AnalyticsUtils.SetTrackingTo((AnalyticsState)Enum.ToObject(typeof(AnalyticsState), newState + 1));
            }

            GUIStyle hyperlink = new GUIStyle();
            hyperlink.normal.textColor = new Color(0.122f, 0.435f, 0.949f);

            GUILayout.BeginArea(new Rect(3, 30, 280, 50));

            if (GUILayout.Button("Data Privacy Information", hyperlink, GUILayout.ExpandWidth(false)))
            {
                AnalyticsUtils.ShowDataPrivacyStatement();
            }

            GUILayout.EndArea();

            // Unity Editor UI has no way to underline text, so this is a fun workaround.
            GUILayout.BeginArea(new Rect(3, 31, 280, 50));

            GUILayout.Label("____________________________", hyperlink);

            GUILayout.EndArea();
        }
    }
}
