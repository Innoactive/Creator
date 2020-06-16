namespace Innoactive.Creator.Core
{
    /// <summary>
    /// Data structure with an <see cref="IStep"/>'s description.
    /// </summary>
    public interface IDescribedData : IData
    {
        /// <summary>
        /// <see cref="IStep"/>'s description.
        /// </summary>
        string Description { get; set; }
    }
}
