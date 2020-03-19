using Innoactive.Creator.Core.Conditions;

namespace Innoactive.Creator.Core
{
    public class BaseAutocompleter<TData> : IAutocompleter<TData> where TData : ICompletableData
    {
        public virtual void Complete(TData data)
        {
            data.IsCompleted = true;
        }
    }
}
