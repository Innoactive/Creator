using VPG.Editor.Configuration;
using UnityEditor;

namespace VPG.Editor.VPGMenu
{
    internal static class SetupSceneEntry
    {
        /// <summary>
        /// Setup the current unity scene to be a functioning training scene.
        /// </summary>
        [MenuItem("VR Process Gizmo/Setup Training Scene", false, 16)]
        public static void SetupScene()
        {
            TrainingSceneSetup.Run();
        }
    }
}
