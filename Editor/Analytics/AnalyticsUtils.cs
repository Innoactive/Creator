using UnityEngine;

namespace Innoactive.CreatorEditor.Analytics
{
    internal class AnalyticsUtils
    {
        private const string KeyTrackingState = "State";

        internal static IAnalyticsTracker CreateTracker()
        {
            return new GoogleTracker();
        }

        internal static AnalyticsState GetTrackingState()
        {
            // No tracking for CI
            if (Application.isBatchMode)
            {
                return AnalyticsState.Disabled;
            }
            return RegistryUtils.GetRegistryEntry(RegistryUtils.AnalyticsEntry,KeyTrackingState, AnalyticsState.Unknown);
        }

        internal static void SetTrackingTo(AnalyticsState state)
        {
            RegistryUtils.SetRegistryEntry(RegistryUtils.AnalyticsEntry,KeyTrackingState, state);
        }
    }
}
