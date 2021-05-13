using VPG.CreatorEditor;
using VPG.CreatorEditor.Analytics;
using UnityEditor;
using UnityEngine;

namespace VPG.Creator.Core.Editor
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

            CreatorProjectSettings settings = CreatorProjectSettings.Load();
            if (settings == null || string.IsNullOrEmpty(settings.ProjectCreatorVersion))
            {
                return;
            }

            if (settings.ProjectCreatorVersion == unknownVersionString || EditorUtils.GetCoreVersion() == unknownVersionString)
            {
                return;
            }

            if (settings.ProjectCreatorVersion != EditorUtils.GetCoreVersion())
            {
                IAnalyticsTracker tracker = AnalyticsUtils.CreateTracker();
                tracker.Send(new AnalyticsEvent() {Category = "creator", Action = "updated", Label = EditorUtils.GetCoreVersion()});
                settings.ProjectCreatorVersion = EditorUtils.GetCoreVersion();
                settings.Save();
            }
        }
    }
}
