using UnityEditor;
using UnityEngine;
using VPG.Core.Configuration;

namespace VPG.Editor
{
    /// <summary>
    /// Will setup a <see cref="RuntimeConfigurator"/> when none is existent in scene.
    /// </summary>
    internal class RuntimeConfigurationSetup : SceneSetup
    {
        public static readonly string TrainingConfiugrationName = "[TRAINING_CONFIGURATION]";
        /// <inheritdoc/>
        public override void Setup()
        {
            if (RuntimeConfigurator.Exists == false)
            {
                GameObject obj = new GameObject(TrainingConfiugrationName);
                obj.AddComponent<RuntimeConfigurator>();
                Selection.activeObject = obj;
            }
        }
    }
}
