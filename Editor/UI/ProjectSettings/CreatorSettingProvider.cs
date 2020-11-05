using UnityEditor;

namespace Innoactive.CreatorEditor.UI
{
    internal class CreatorSettingProvider : BaseSettingsProvider
    {
        const string Path = "Project/Creator/Settings";

        public CreatorSettingProvider() : base(Path, SettingsScope.Project)
        {
        }

        protected override void InternalDraw(string searchContext)
        {

        }

        [SettingsProvider]
        public static SettingsProvider Provider()
        {
            SettingsProvider provider = new CreatorSettingProvider();
            return provider;
        }
    }
}
