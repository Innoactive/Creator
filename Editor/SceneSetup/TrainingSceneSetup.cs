using Innoactive.Creator.Unity;

namespace Innoactive.CreatorEditor
{
    /// <summary>
    /// Scene setup for training configuration.
    /// </summary>
    public class TrainingSceneSetup : SceneSetup
    {
        /// <inheritdoc />
        public override void Setup()
        {
            SceneUtils.SetupTrainingConfiguration();
        }
    }
}
