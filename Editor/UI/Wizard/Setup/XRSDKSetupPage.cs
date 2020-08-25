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

        private readonly List<XRLoader> options = new List<XRLoader>(Enum.GetValues(typeof(XRLoader)).Cast<XRLoader>());

        private readonly List<string> nameplates = new List<string>()
        {
            "None",
            "HTC Vive / Valve Index (OpenVR)",
            "Oculus Quest/Rift",
            "Windows MR",
            "Other"
        };

        [SerializeField]
        private XRLoader selectedLoader = XRLoader.None;

        [SerializeField]
        private string otherHardwareText = null;

        public XRSDKSetupPage() : base("XR Hardware")
        {

        }

        /// <inheritdoc/>
        public override void Draw(Rect window)
        {
            GUILayout.BeginArea(window);
            {
                GUILayout.Label("VR Hardware Setup", CreatorEditorStyles.Title);
                GUILayout.Label("Select the VR hardware you are working with:", CreatorEditorStyles.Header);
                selectedLoader = CreatorGUILayout.DrawToggleGroup(selectedLoader, options, nameplates);

                if (selectedLoader == XRLoader.Other)
                {
                    GUILayout.Label("The Creator does not provide an automated setup for your device. You need to refer to your device's vendor documentation in order to enable a compatible loader in the Unity's XR Plugin Management.", CreatorEditorStyles.Paragraph);

                    GUILayout.BeginHorizontal();
                        GUILayout.Label("Please tell us which VR Hardware you are using:", CreatorEditorStyles.Label);
                        otherHardwareText = CreatorGUILayout.DrawTextField(otherHardwareText, -1,GUILayout.Width(window.width * 0.4f));
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndArea();
        }

        /// <inheritdoc/>
        public override void Skip()
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
