using VPG.Editor;
using VPG.Editor.Analytics;
using UnityEditor;
using UnityEngine;

namespace VPG.Core.Editor
{
    /// <summary>
    /// Checks if the version of the VR Process Gizmo was updated and sends an event.
    /// </summary>
    [InitializeOnLoad]
    internal static class VersionCheckerEvent
    {
        private const string unknownVersionString = "unknown";

        static VersionCheckerEvent()
        {
            if (Application.isBatchMode)
            {
                return;
            }

            VPGProjectSettings settings = VPGProjectSettings.Load();
            if (settings == null || string.IsNullOrEmpty(settings.ProjectVPGVersion))
            {
                return;
            }

            if (settings.ProjectVPGVersion == unknownVersionString || EditorUtils.GetCoreVersion() == unknownVersionString)
            {
                return;
            }

            if (settings.ProjectVPGVersion != EditorUtils.GetCoreVersion())
            {
                IAnalyticsTracker tracker = AnalyticsUtils.CreateTracker();
                tracker.Send(new AnalyticsEvent() {Category = "creator", Action = "updated", Label = EditorUtils.GetCoreVersion()});
                settings.ProjectVPGVersion = EditorUtils.GetCoreVersion();
                settings.Save();
            }
        }
    }
}
