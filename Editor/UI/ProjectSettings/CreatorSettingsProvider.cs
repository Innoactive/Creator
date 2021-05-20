using UnityEditor;

namespace VPG.Editor.UI
{
    internal class CreatorSettingsProvider : BaseSettingsProvider
    {
        const string Path = "Project/Creator/Settings";

        public CreatorSettingsProvider() : base(Path, SettingsScope.Project)
        {
        }

        protected override void InternalDraw(string searchContext)
        {

        }

        [SettingsProvider]
        public static SettingsProvider Provider()
        {
            SettingsProvider provider = new CreatorSettingsProvider();
            return provider;
        }
    }
}
