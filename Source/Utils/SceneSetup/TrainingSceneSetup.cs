using Innoactive.Hub.Training.Unity.Utils;

namespace Innoactive.Creator.Utils
{
    /// <summary>
    /// Scene setup for training configuration.
    /// </summary>
    public class TrainingSceneSetup : OnSceneSetup
    {
        /// <inheritdoc />
        public override void Setup()
        {
            SceneUtils.SetupTrainingConfiguration();
        }
    }
}
