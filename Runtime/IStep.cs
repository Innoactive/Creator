namespace VPG.Creator.Core
{
    /// <summary>
    /// Interface for a training step.
    /// </summary>
    public interface IStep : IDataOwner<IStepData>, IEntity
    {
        /// <summary>
        /// Step's metadata.
        /// </summary>
        StepMetadata StepMetadata { get; set; }
    }
}
