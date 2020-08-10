using System;
using UnityEditor;
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
            if (state == AnalyticsState.Minimal)
            {
                // Without a stored session id a random one will be created every time.
                if (EditorPrefs.HasKey(BaseAnalyticsTracker.KeySessionId))
                {
                    EditorPrefs.DeleteKey(BaseAnalyticsTracker.KeySessionId);
                }
            }

            if (state == AnalyticsState.Enabled)
            {
                string id = EditorPrefs.GetString(BaseAnalyticsTracker.KeySessionId, null);
                if (string.IsNullOrEmpty(id) || id.StartsWith("IA") == false)
                {
                    // Create new session id starting with IA, which allows better tracking
                    EditorPrefs.SetString(BaseAnalyticsTracker.KeySessionId,
                        "IA" + Guid.NewGuid().ToString().Substring(2));
                }
            }

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
