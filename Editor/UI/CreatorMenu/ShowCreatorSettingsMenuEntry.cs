using VPG.Editor.Configuration;
using UnityEditor;

namespace VPG.Editor.VPGMenu
{
    internal static class ShowCreatorSettingsMenuEntry
    {
        /// <summary>
        /// Setup the current unity scene to be a functioning training scene.
        /// </summary>
        [MenuItem("VR Process Gizmo/Settings", false, 16)]
        public static void Show()
        {
            SettingsService.OpenProjectSettings("Project/VR Process Gizmo");
        }
    }
}
