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

            if (trackingState == AnalyticsState.Minimal)
            {
                // Without a stored session id a random one will be created every time.
                if (EditorPrefs.HasKey(BaseAnalyticsTracker.KeySessionId))
                {
                    EditorPrefs.DeleteKey(BaseAnalyticsTracker.KeySessionId);
                }
            }

            if (trackingState == AnalyticsState.Enabled)
            {
                string id = EditorPrefs.GetString(BaseAnalyticsTracker.KeySessionId, null);
                if (string.IsNullOrEmpty(id) || id.StartsWith("IA") == false)
                {
                    // Create new session id starting with IA, which allows better tracking
                    EditorPrefs.SetString(BaseAnalyticsTracker.KeySessionId,
                        "IA" + Guid.NewGuid().ToString().Substring(2));
                }
            }

            // Only run once a day.
            if (DateTime.Today.Ticks.ToString().Equals(EditorPrefs.GetString(KeyLastDayActive, null)) == false)
            {
                EditorPrefs.SetString(KeyLastDayActive, DateTime.Today.Ticks.ToString());
                IAnalyticsTracker tracker = AnalyticsUtils.CreateTracker();
                // Send "hello".
                tracker.Send(new AnalyticsEvent() {Category = "system", Action = "hello", Label = ""});
                // Send the Unity Editor version.
                tracker.Send(new AnalyticsEvent() {Category = "unity", Action = "version", Label = Application.unityVersion});
                // Send the Creator Core version.
                tracker.Send(new AnalyticsEvent() {Category = "creator", Action = "version", Label = EditorUtils.GetCoreVersion()});
            }
        }
    }
}
