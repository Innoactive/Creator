using System;
using VPG.Core.Utils.Logging;
using UnityEditor;
using UnityEngine;

namespace VPG.Editor.UI
{
    internal class LoggingSettingsSection : IProjectSettingsSection
    {
        public string Title { get; } = "Course LifeCycle Logging";
        public Type TargetPageProvider { get; } = typeof(VPGSettingsProvider);
        public int Priority { get; } = 1000;

        public void OnGUI(string searchContext)
        {
            LifeCycleLoggingConfig config = LifeCycleLoggingConfig.Instance;

            EditorGUI.BeginChangeCheck();

            config.LogChapters = GUILayout.Toggle(config.LogChapters, "Log Chapter output", VPGEditorStyles.Toggle);
            config.LogSteps = GUILayout.Toggle(config.LogSteps, "Log Step output", VPGEditorStyles.Toggle);
            config.LogBehaviors = GUILayout.Toggle(config.LogBehaviors, "Log Behaviors output", VPGEditorStyles.Toggle);
            config.LogTransitions = GUILayout.Toggle(config.LogTransitions, "Log Transition output", VPGEditorStyles.Toggle);
            config.LogConditions = GUILayout.Toggle(config.LogConditions, "Log Condition output", VPGEditorStyles.Toggle);
        }

        ~LoggingSettingsSection()
        {
            if (EditorUtility.IsDirty(LifeCycleLoggingConfig.Instance))
            {
                LifeCycleLoggingConfig.Instance.Save();
            }
        }
    }
}
