using Innoactive.CreatorEditor.Configuration;
using UnityEditor;

namespace Innoactive.CreatorEditor.CreatorMenu
{
    internal static class SetupSceneEntry
    {
        /// <summary>
        /// Setup the current unity scene to be a functioning training scene.
        /// </summary>
        [MenuItem("Innoactive/Setup Training Scene", false, 16)]
        public static void SetupScene()
        {
            TrainingSceneSetup.Run();
        }
    }
}
