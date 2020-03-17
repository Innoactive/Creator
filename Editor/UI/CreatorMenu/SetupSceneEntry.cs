using Innoactive.CreatorEditor.Configuration;
using UnityEditor;

namespace Innoactive.CreatorEditor.CreatorMenu
{
    public static class SetupSceneEntry
    {
        /// <summary>
        /// Setup the current unity scene to be a functioning training scene.
        /// </summary>
        [MenuItem("Innoactive/Creator/Setup Training Scene", false, 10)]
        public static void SetupScene()
        {
            EditorConfigurator.Instance.SetupTrainingScene();
        }
    }
}
