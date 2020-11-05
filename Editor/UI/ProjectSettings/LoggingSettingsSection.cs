using System;
using Innoactive.Creator.Core.Utils.Logging;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI
{
    internal class LoggingSettingsSection : IProjectSettingsSection
    {
        public string Title { get; } = "Course LifeCycle Logging";
        public Type TargetPageProvider { get; } = typeof(CreatorSettingProvider);
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

            if (EditorGUI.EndChangeCheck())
            {
                config.Save();
            }
        }
    }
}
