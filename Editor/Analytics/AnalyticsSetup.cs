using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.Analytics
{
    /// <summary>
    /// Checks on each recompile/start of the unity if we have already sent a hello.
    /// Adding -no-tracking when starting unity will disable analytics automatically.
    /// </summary>
    [InitializeOnLoad]
    internal class AnalyticsSetup
    {
        private const string KeyLastDayActive = "Innoactive.Creator.Analytics.LastDayActive";

        static AnalyticsSetup()
        {
            AnalyticsState trackingState = AnalyticsUtils.GetTrackingState();
            if (trackingState == AnalyticsState.Disabled)
            {
                return;
            }
            // Can be used by ci to deactivate tracking.
            if (Environment.GetCommandLineArgs().Contains("-no-tracking"))
            {
                AnalyticsUtils.SetTrackingTo(AnalyticsState.Disabled);
                return;
            }

            if (trackingState == AnalyticsState.Unknown)
            {
                SetupTrackingPopup.Open();
                AnalyticsUtils.SetTrackingTo(AnalyticsState.Minimal);
                return;
            }

            // Only run once a day.
            if (DateTime.Today.Ticks.ToString().Equals(EditorPrefs.GetString(KeyLastDayActive, null)) == false)
            {
                EditorPrefs.SetString(KeyLastDayActive, DateTime.Today.Ticks.ToString());
                IAnalyticsTracker tracker = AnalyticsUtils.CreateTracker();
                // Send the Unity Editor version.
                tracker.Send(new AnalyticsEvent() {Category = "unity", Action = "version", Label = Application.unityVersion});
                // Send the Creator Core version.
                tracker.Send(new AnalyticsEvent() {Category = "creator", Action = "version", Label = EditorUtils.GetCoreVersion()});
            }
        }
    }
}
