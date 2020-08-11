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
            OpenVR,
            Oculus,
            WindowsMR,
            Other,
            None
        }

        private readonly List<XRLoader> options = new List<XRLoader>(Enum.GetValues(typeof(XRLoader)).Cast<XRLoader>());

        private readonly List<string> nameplates = new List<string>()
        {
            "HTC Vive / Valve Index (OpenVR)",
            "Oculus Quest/Rift",
            "Windows MR",
            "Other",
            "None"
        };

        private IDictionary<XRLoader, string> infos = new Dictionary<XRLoader, string>
        {
            { XRLoader.OpenVR, "OpenVR XR Plugin will be imported into the project." },
            { XRLoader.Oculus, "Oculus XR Plugin will be imported into the project." },
            { XRLoader.WindowsMR, "Windows XR Plugin will be imported into the project." },
            { XRLoader.Other, "Right now we do not support other than the listed plugins." },
            { XRLoader.None, "If you dont want to import any XR related plugins, press the skip button." }
        };

        private GUIContent infoContent;

        [SerializeField]
        private XRLoader selectedLoader = XRLoader.None;

        [SerializeField]
        private string otherHardwareText = null;

        public XRSDKSetupPage() : base("XR Hardware", true)
        {
            CanProceed = false;
            infoContent = EditorGUIUtility.IconContent("console.infoicon.inactive.sml");
        }

        /// <inheritdoc/>
        public override void Draw(Rect window)
        {
            GUILayout.BeginArea(window);
            {
                GUILayout.Label("Select VR Hardware", CreatorEditorStyles.Title);

                infoContent.text = infos[selectedLoader];
                EditorGUILayout.LabelField(infoContent, CreatorEditorStyles.ApplyMargin(CreatorEditorStyles.Label));

                selectedLoader = CreatorGUILayout.DrawToggleGroup(selectedLoader, options, nameplates);

                if (selectedLoader == XRLoader.Other)
                {
                    GUILayout.Label("Which VR Hardware are you using?");
                    otherHardwareText = CreatorGUILayout.DrawTextField(otherHardwareText, -1, GUILayout.Width(window.width * 0.7f));
                }

                CanProceed = selectedLoader != XRLoader.None;
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

                AnalyticsUtils.CreateTracker().Send(hardwareSelectedEvent);

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

        private void ResetSettings()
        {
            CanProceed = false;
            selectedLoader = XRLoader.None;
            otherHardwareText = string.Empty;
        }
    }
}
