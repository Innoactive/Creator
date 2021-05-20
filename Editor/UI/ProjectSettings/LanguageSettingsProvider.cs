using VPG.Core.Internationalization;
using UnityEditor;

namespace VPG.Editor.UI
{
    public class LanguageSettingsProvider : BaseSettingsProvider
    {
        const string Path = "Project/Creator/Language";

        public LanguageSettingsProvider() : base(Path, SettingsScope.Project) {}

        protected override void InternalDraw(string searchContext)
        {
            LanguageSettings config = LanguageSettings.Instance;
            UnityEditor.Editor.CreateEditor(config).OnInspectorGUI();
        }

        public override void OnDeactivate()
        {
            if (EditorUtility.IsDirty(LanguageSettings.Instance))
            {
                LanguageSettings.Instance.Save();
            }
        }

        [SettingsProvider]
        public static SettingsProvider Provider()
        {
            SettingsProvider provider = new LanguageSettingsProvider();
            return provider;
        }
    }
}
