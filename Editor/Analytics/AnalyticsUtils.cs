using System;
using UnityEditor;
using UnityEngine;

namespace VPG.CreatorEditor.Analytics
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

            try
            {
                return EditorPrefExtensions.GetEnum(KeyTrackingState, AnalyticsState.Unknown);
            }
            catch (ArgumentException)
            {
                EditorPrefExtensions.SetEnum(KeyTrackingState, AnalyticsState.Unknown);
                return AnalyticsState.Unknown;
            }
        }

        internal static void SetTrackingTo(AnalyticsState state)
        {
            // No tracking for CI
            if (Application.isBatchMode)
            {
                return;
            }

            AnalyticsState currentState = GetTrackingState();
            if (state == AnalyticsState.Enabled && currentState != AnalyticsState.Enabled)
            {
                string id = EditorPrefs.GetString(BaseAnalyticsTracker.KeySessionId, null);
                if (string.IsNullOrEmpty(id))
                {
                    // Create new session id, which allows better tracking
                    EditorPrefs.SetString(BaseAnalyticsTracker.KeySessionId, Guid.NewGuid().ToString());
                }
            }

            if (currentState != state)
            {
                SendTrackingEvent(state);
                EditorPrefExtensions.SetEnum(KeyTrackingState, state);
            }
        }

        private static void SendTrackingEvent(AnalyticsState state)
        {
            CreateTracker().Send(new AnalyticsEvent()
            {
                Action = "creator",
                Category = "tracking",
                Label = state.ToString().ToLower()
            });
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
