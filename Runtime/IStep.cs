namespace Innoactive.Hub.Training
{
    /// <summary>
    /// Interface for a training step.
    /// </summary>
    public interface IStep : IDataOwner<IStepData>, IEntity
    {
        StepMetadata StepMetadata { get; set; }
    }
}
