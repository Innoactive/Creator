using UnityEditor;
using UnityEngine;
using Innoactive.Creator.Core.Configuration;

namespace Innoactive.CreatorEditor
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
