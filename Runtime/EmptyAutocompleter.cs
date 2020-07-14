namespace Innoactive.Creator.Core
{
    /// <summary>
    /// An autocompleter that does nothing.
    /// </summary>
    public class EmptyAutocompleter : IAutocompleter
    {
        /// <inheritdoc />
        public void Complete()
        {
        }
    }
}
