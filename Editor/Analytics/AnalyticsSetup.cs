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

            List<string> args = Environment.GetCommandLineArgs().ToList();
            if (args.Contains("-no-tracking"))
            {
                AnalyticsUtils.SetTrackingTo(AnalyticsState.Disabled);
            }
            else if (trackingState == AnalyticsState.Unknown)
            {
                SetupTrackingPopup.ShowWindow();
            }
            else if (trackingState >= AnalyticsState.Minimum)
            {
                // Only run once a day.
                if (DateTime.Today.Ticks != EditorPrefs.GetFloat(KeyLastDayActive, 0L))
                {
                    EditorPrefs.SetFloat(KeyLastDayActive, DateTime.Today.Ticks);
                    // Create new Session id
                    EditorPrefs.SetString(BaseAnalyticsTracker.KeySessionId, Guid.NewGuid().ToString());
                    IAnalyticsTracker tracker = AnalyticsUtils.CreateTracker();
                    // Send simple hallo
                    tracker.Send(new AnalyticsEvent() {Category = "system", Action = "hello", Label = ""});
                    // Send unity version used
                    tracker.Send(new AnalyticsEvent() {Category = "unity", Action = "version", Label = Application.unityVersion});
                    // Send creator core version used
                    tracker.Send(new AnalyticsEvent() {Category = "creator", Action = "version", Label = EditorUtils.GetCoreVersion()});
                }
            }
        }
    }
}
