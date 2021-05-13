namespace VPG.Creator.Core
{
    /// <summary>
    /// A base interface for an autocompleter. Autocompleters are used alongside fast-forwarding of the processes. They must fake circumstances under which conditions should complete if fast-forwarding requires it.
    /// </summary>
    public interface IAutocompleter
    {
        /// <summary>
        /// A custom logic to "fake" natural completion of an entity.
        /// </summary>
        void Complete();
    }
}
