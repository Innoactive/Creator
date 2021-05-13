using UnityEditor;
using UnityEngine;

namespace VPG.CreatorEditor.Analytics
{
    internal class SetupTrackingPopup : EditorWindow
    {
        private static SetupTrackingPopup instance;

        public static void Open()
        {
            EditorApplication.update += ShowWindow;
        }

        private static void ShowWindow()
        {
            EditorApplication.update -= ShowWindow;
            if (instance == null)
            {
                instance = GetWindow<SetupTrackingPopup>(true);
                AssemblyReloadEvents.beforeAssemblyReload += HideWindow;
                instance.ShowUtility();
            }

            instance.minSize = new Vector2(440f, 360f);
            instance.maxSize = new Vector2(440f, 360f);
            instance.Focus();
        }

        private static void HideWindow()
        {
            instance.Close();
            instance = null;

            AssemblyReloadEvents.beforeAssemblyReload -= HideWindow;
        }

        private void OnGUI()
        {
            titleContent = new GUIContent("Usage statistics");

            EditorGUILayout.Space(4f);

            GUIStyle style = GUI.skin.box;
            style.alignment = TextAnchor.UpperLeft;
            GUILayout.Box("To improve the Creator, Innoactive collects usage data. We are committed to protect our users' privacy and do not collect any sensitive information like source code, file names or your courses' structure. The data collected encompasses:\n\n- exact version of Innoactive Creator\n- exact version of Unity Editor\n- your system's language\n- information about usage of the Innoactive Creators' components\n\nIn order to track the above information we store a unique identifier within Unity's Editor Preferences. Since the Creator is an open-source project, you can check the source code of our analytics engine at any time in the following folder: Core/Editor/Analytics\n\nIf you want to completely opt-out of tracking, open Innoactive > Settings > Analytics Settings in the Unity's menu bar.\n\nBy clicking the \"Accept\" button below, you agree on the terms described above. To learn more about Innoactive's terms, also check:\n", style, GUILayout.Width(432f));

            EditorGUILayout.Space(4f);

            if (GUILayout.Button("Accept"))
            {
                AnalyticsUtils.SetTrackingTo(AnalyticsState.Enabled);
                Close();
            }

            GUIStyle hyperlink = new GUIStyle();
            hyperlink.normal.textColor = new Color(0.122f, 0.435f, 0.949f);
            Rect positionRect = new Rect(6, 310, 280, 50);

            GUILayout.BeginArea(positionRect);

            if (GUILayout.Button("Data Privacy Information", hyperlink, GUILayout.ExpandWidth(false)))
            {
                AnalyticsUtils.ShowDataPrivacyStatement();
            }

            GUILayout.EndArea();

            // Unity Editor UI has no way to underline text, so this is a fun workaround.
            positionRect.y += 1;
            GUILayout.BeginArea(positionRect);

            GUILayout.Label("____________________________", hyperlink);

            GUILayout.EndArea();
        }
    }

}
