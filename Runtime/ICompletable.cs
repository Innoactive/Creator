namespace VPG.Core
{
    /// <summary>
    /// Base interface for objects which can be completed.
    /// </summary>
    public interface ICompletable
    {
        /// <summary>
        /// True if this instance is already completed.
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// Forces this instance's completion.
        /// </summary>
        void Autocomplete();
    }
}
