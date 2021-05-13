using UnityEditor;
using UnityEngine;

namespace VPG.CreatorEditor.UI
{
    internal class CreatorPageProvider : BaseSettingsProvider
    {
        const string Path = "Project/Creator";

        public CreatorPageProvider() : base(Path, SettingsScope.Project)
        {
        }

        protected override void InternalDraw(string searchContext)
        {

        }

        [SettingsProvider]
        public static SettingsProvider GetCreatorSettingsProvider()
        {
            SettingsProvider provider = new CreatorPageProvider();
            return provider;
        }
    }
}
