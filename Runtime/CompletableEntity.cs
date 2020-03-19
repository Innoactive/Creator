using Innoactive.Creator.Core.Conditions;

namespace Innoactive.Creator.Core
{
    public abstract class CompletableEntity<TData> : Entity<TData>, ICompletableEntity where TData : ICompletableData
    {
        protected abstract IAutocompleter<TData> Autocompleter { get; }

        public bool IsCompleted
        {
            get
            {
                return Data.IsCompleted;
            }
        }

        public void Autocomplete()
        {
            if (LifeCycle.Stage == Stage.Active)
            {
                Autocompleter.Complete(Data);
            }
        }
    }
}
