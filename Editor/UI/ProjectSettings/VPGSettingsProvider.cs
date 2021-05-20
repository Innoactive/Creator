using UnityEditor;

namespace VPG.Editor.UI
{
    internal class VPGSettingsProvider : BaseSettingsProvider
    {
        const string Path = "Project/VR Process Gizmo/Settings";

        public VPGSettingsProvider() : base(Path, SettingsScope.Project)
        {
        }

        protected override void InternalDraw(string searchContext)
        {

        }

        [SettingsProvider]
        public static SettingsProvider Provider()
        {
            SettingsProvider provider = new VPGSettingsProvider();
            return provider;
        }
    }
}
