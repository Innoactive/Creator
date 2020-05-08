using UnityEngine;

namespace Innoactive.CreatorEditor.Analytics
{
    internal class AnalyticsUtils
    {
        private const string KeyTrackingState = "Innoactive.Creator.Analytics.State";

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
            return EditorPrefExtensions.GetEnum(KeyTrackingState, AnalyticsState.Unknown);
        }

        internal static void SetTrackingTo(AnalyticsState state)
        {
            EditorPrefExtensions.SetEnum(KeyTrackingState, state);
        }
    }
}
