using System;
using VPG.Core.Utils.Logging;
using UnityEditor;
using UnityEngine;

namespace VPG.Editor.UI
{
    internal class LoggingSettingsSection : IProjectSettingsSection
    {
        public string Title { get; } = "Course LifeCycle Logging";
        public Type TargetPageProvider { get; } = typeof(CreatorSettingsProvider);
        public int Priority { get; } = 1000;

        public void OnGUI(string searchContext)
        {
            LifeCycleLoggingConfig config = LifeCycleLoggingConfig.Instance;

            EditorGUI.BeginChangeCheck();

            config.LogChapters = GUILayout.Toggle(config.LogChapters, "Log Chapter output", CreatorEditorStyles.Toggle);
            config.LogSteps = GUILayout.Toggle(config.LogSteps, "Log Step output", CreatorEditorStyles.Toggle);
            config.LogBehaviors = GUILayout.Toggle(config.LogBehaviors, "Log Behaviors output", CreatorEditorStyles.Toggle);
            config.LogTransitions = GUILayout.Toggle(config.LogTransitions, "Log Transition output", CreatorEditorStyles.Toggle);
            config.LogConditions = GUILayout.Toggle(config.LogConditions, "Log Condition output", CreatorEditorStyles.Toggle);
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
