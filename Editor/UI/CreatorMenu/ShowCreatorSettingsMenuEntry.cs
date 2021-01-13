using Innoactive.CreatorEditor.Configuration;
using UnityEditor;

namespace Innoactive.CreatorEditor.CreatorMenu
{
    internal static class ShowCreatorSettingsMenuEntry
    {
        /// <summary>
        /// Setup the current unity scene to be a functioning training scene.
        /// </summary>
        [MenuItem("Innoactive/Settings", false, 16)]
        public static void Show()
        {
            SettingsService.OpenProjectSettings("Project/Creator");
        }
    }
}
