using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using Innoactive.CreatorEditor.XRUtils;
using Innoactive.CreatorEditor.Analytics;

namespace Innoactive.CreatorEditor.UI.Wizard
{
    /// <summary>
    /// Wizard page which retrieves and loads XR SDKs.
    /// </summary>
    internal class XRSDKSetupPage : WizardPage
    {
        private enum XRLoader
        {
            None,
            OpenVR,
            Oculus,
            WindowsMR,
            Other
        }

        private const string OpenVRInfo = "OpenVR XR Plugin will be imported into the project.";
        private const string OculusInfo = "Oculus XR Plugin will be imported into the project.";
        private const string WindowsMRInfo = "Windows XR Plugin will be imported into the project.";

        private GUIContent infoContent;
        private string otherHardwareText;
        private XRLoader selectedLoader = XRLoader.None;
        private Dictionary<XRLoader, bool> settings = new Dictionary<XRLoader, bool>();

        public XRSDKSetupPage() : base("Step 2: XR Hardware", true)
        {
            CanProceed = false;

            foreach (XRLoader loader in Enum.GetValues(typeof(XRLoader)))
            {
                settings.Add(loader, false);
            }

            infoContent = EditorGUIUtility.IconContent("console.infoicon.inactive.sml");
        }

        /// <inheritdoc/>
        public override void Draw(Rect window)
        {
            GUILayout.BeginArea(window);
            {
                // Title
                GUILayout.Label("Select VR Hardware", CreatorEditorStyles.Title);

                // OpenVR
                GUILayout.BeginHorizontal(CreatorEditorStyles.Paragraph);
                {
                    if (DrawLoaderOption("HTC", XRLoader.OpenVR))
                    {
                        infoContent.text = OpenVRInfo;
                        EditorGUILayout.LabelField(infoContent);
                    }
                }
                GUILayout.EndHorizontal();

                // Oculus
                GUILayout.BeginHorizontal(CreatorEditorStyles.Paragraph);
                {
                    if (DrawLoaderOption("Oculus", XRLoader.Oculus))
                    {
                        infoContent.text = OculusInfo;
                        EditorGUILayout.LabelField(infoContent);
                    }
                }
                GUILayout.EndHorizontal();

                // Windows MR
                GUILayout.BeginHorizontal(CreatorEditorStyles.Paragraph);
                {
                    if (DrawLoaderOption("Windows", XRLoader.WindowsMR))
                    {
                        infoContent.text = WindowsMRInfo;
                        EditorGUILayout.LabelField(infoContent);
                    }
                }
                GUILayout.EndHorizontal();

                // Other
                GUILayout.BeginHorizontal(CreatorEditorStyles.Paragraph);
                {
                    DrawLoaderOption("Other", XRLoader.Other);
                }
                GUILayout.EndHorizontal();

                if (settings[XRLoader.Other])
                {
                    GUILayout.BeginVertical(CreatorEditorStyles.Paragraph);
                    {
                        GUILayout.Label("Which VR Hardware are you using?");
                        otherHardwareText = GUILayout.TextField(otherHardwareText);
                    }
                    GUILayout.EndVertical();
                }
            }
            GUILayout.EndArea();
        }

        /// <inheritdoc/>
        public override void Apply()
        {
            if (selectedLoader == XRLoader.Other)
            {
                string message = "The Creator does not provide an automated setup for your device. You need to refer to your device's vendor documentation in order to enable a compatible loader in the Unity's XR Plugin Management.";
                EditorUtility.DisplayDialog("Device not loaded.", message, "Understood");
            }
        }

        /// <inheritdoc/>
        public override void Skip()
        {
            ResetSettings();
        }

        /// <inheritdoc/>
        public override void Back()
        {
            ResetSettings();
        }

        /// <inheritdoc/>
        public override void Closing(bool isCompleted)
        {
            if (isCompleted)
            {
                AnalyticsEvent hardwareSelectedEvent = new AnalyticsEvent
                {
                    Category = "creator",
                    Action = "hardware_selected",
                    Label = selectedLoader == XRLoader.Other ? otherHardwareText : selectedLoader.ToString()
                };

                //AnalyticsUtils.CreateTracker().Send(hardwareSelectedEvent);

                switch (selectedLoader)
                {
                    case XRLoader.Oculus:
                        XRLoaderHelper.LoadOculus();
                        break;
                    case XRLoader.OpenVR:
                        XRLoaderHelper.LoadOpenVR();
                        break;
                    case XRLoader.WindowsMR:
                        XRLoaderHelper.LoadWindowsMR();
                        break;
                }
            }
        }

        private bool DrawLoaderOption(string label, XRLoader targetLoader)
        {
            if (GUILayout.Toggle(settings[targetLoader], label))
            {
                settings.Keys.ToList().ForEach(loader => settings[loader] = loader == targetLoader);
                selectedLoader = targetLoader;
                CanProceed = true;
            }

            return selectedLoader == targetLoader;
        }

        private void ResetSettings()
        {
            CanProceed = false;
            selectedLoader = XRLoader.None;
            otherHardwareText = string.Empty;
            settings.Keys.ToList().ForEach(loader => settings[loader] = default);
        }
    }
}
