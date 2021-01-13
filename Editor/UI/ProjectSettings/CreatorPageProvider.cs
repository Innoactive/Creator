using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI
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
