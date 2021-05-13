using VPG.Creator.Core.SceneObjects;

namespace VPG.Creator.Core.Properties
{
    /// <summary>
    /// Used to identify a trainee within the scene.
    /// </summary>
    public class TraineeSceneObject : TrainingSceneObject
    {
        protected new void Awake()
        {
            base.Awake();
            ChangeUniqueName("Trainee");
        }
    }
}
