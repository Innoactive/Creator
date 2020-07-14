namespace Innoactive.Creator.Core
{
    /// <summary>
    /// A base class for autocompleters which provides access to the entity's data.
    /// </summary>
    public abstract class Autocompleter<TData> : IAutocompleter where TData : IData
    {
        /// <summary>
        /// The entity's data.
        /// </summary>
        protected TData Data { get; }

        protected Autocompleter(TData data)
        {
            Data = data;
        }

        ///<inheritdoc />
        public abstract void Complete();
    }
}
