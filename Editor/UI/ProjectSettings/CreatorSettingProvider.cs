using Innoactive.Creator.Core.Utils.Logging;
using Innoactive.CreatorEditor.UI;
using UnityEditor;
using UnityEngine;

internal class CreatorSettingProvider : SettingsProvider
{
    const string Path = "Project/Creator/Settings";

    public CreatorSettingProvider() : base(Path, SettingsScope.Project) {}

    public override void OnGUI(string searchContext)
    {
        GUILayout.Space(8);
        GUILayout.Label("Selected output will be logged into the console:", CreatorEditorStyles.ApplyPadding(CreatorEditorStyles.Paragraph, 0));
        LifeCycleLoggingConfig config = LifeCycleLoggingConfig.Instance;
        if (Editor.CreateEditor(config).DrawDefaultInspector())
        {
            config.Save();
        }
    }

    [SettingsProvider]
    public static SettingsProvider Provider()
    {
        SettingsProvider provider = new CreatorSettingProvider();
        return provider;
    }
}
