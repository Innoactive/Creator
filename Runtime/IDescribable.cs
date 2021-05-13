namespace VPG.Creator.Core
{
    /// <summary>
    /// Interface for all training entities that have a description.
    /// </summary>
    public interface IDescribable
    {
        /// <summary>
        /// Description of this training entity.
        /// </summary>
        string Description { get; set; }
    }
}
