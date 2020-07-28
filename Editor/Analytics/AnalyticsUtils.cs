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

        internal static void ShowDataPrivacyStatement()
        {
            string pathToDataPrivacyStatement = "/.Documentation/articles/innoactive-creator/data-privacy-information.md";
            if (EditorUtils.GetCoreVersion().Equals("unknown"))
            {
                string dataPrivacyStatementUrl = EditorUtils.GetCoreFolder() + pathToDataPrivacyStatement;
                UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(dataPrivacyStatementUrl, 1);
            }
            else
            {
                string dataPrivacyStatementUrl = $"https://github.com/Innoactive/Creator/tree/{EditorUtils.GetCoreVersion()}{pathToDataPrivacyStatement}";
                Application.OpenURL(dataPrivacyStatementUrl);
            }
        }
    }
}
