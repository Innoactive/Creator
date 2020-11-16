namespace Innoactive.Creator.Core.Runtime.Properties
{
    /// <summary>
    /// Interface which allows validation to check if the object validated is empty.
    /// </summary>
    public interface ICanBeEmpty
    {
        /// <summary>
        /// Returns true when this object is not properly filled.
        /// </summary>
        bool IsEmpty();
    }
}
