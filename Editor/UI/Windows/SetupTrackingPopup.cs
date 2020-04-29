using System;
using System.IO;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Configuration;
using Innoactive.CreatorEditor.UI;
using Innoactive.CreatorEditor.UI.Windows;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.Analytics
{
    internal class SetupTrackingPopup : EditorWindow
    {
        private static SetupTrackingPopup instance;

        public static void ShowWindow()
        {
            if (instance == null)
            {
                instance = CreateInstance<SetupTrackingPopup>();

                Rect position = new Rect(0, 0, 280f, 240f);
                position.center = new Rect(0f, 0f, Screen.currentResolution.width, Screen.currentResolution.height)
                    .center;
                instance.position = position;

                AssemblyReloadEvents.beforeAssemblyReload += HideWindow;

                instance.ShowUtility();
            }


            instance.minSize = new Vector2(280f, 238f);
            instance.maxSize = new Vector2(280f, 238f);
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

            EditorGUILayout.Space(8f);
            EditorGUILayout.HelpBox(new GUIContent("To help Innoactive improve the Creator we are sending anonymous data about your software configuration. Right now we are tracking:\n\n * Creator version\n * Unity version\n * System Language\n\nThe collected data exclude any sensitive data like source code, file names, created course structure.\n\nYou can check the source code of our analytics engine in this folder: Core/Editor/Analytics\n\nIf you want to disable tracking go to Innoactive/Creator/Windows/Analytics Settings"));
            EditorGUILayout.Space(4f);

            if (GUILayout.Button("Accept"))
            {
                AnalyticsUtils.SetTrackingTo(AnalyticsState.Enabled);
                Close();
            }
        }
    }
}
