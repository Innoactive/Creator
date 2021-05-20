using UnityEditor;
using UnityEngine;

namespace VPG.Editor.UI
{
    internal class VPGPageProvider : BaseSettingsProvider
    {
        const string Path = "Project/VR Process Gizmo";

        public VPGPageProvider() : base(Path, SettingsScope.Project)
        {
        }

        protected override void InternalDraw(string searchContext)
        {

        }

        [SettingsProvider]
        public static SettingsProvider GetVPGSettingsProvider()
        {
            SettingsProvider provider = new VPGPageProvider();
            return provider;
        }
    }
}
