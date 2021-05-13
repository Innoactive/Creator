using VPG.CreatorEditor.Configuration;
using UnityEditor;

namespace VPG.CreatorEditor.CreatorMenu
{
    internal static class ShowCreatorSettingsMenuEntry
    {
        /// <summary>
        /// Setup the current unity scene to be a functioning training scene.
        /// </summary>
        [MenuItem("VR Process Gizmo/Settings", false, 16)]
        public static void Show()
        {
            SettingsService.OpenProjectSettings("Project/Creator");
        }
    }
}
