using Innoactive.Hub.Training.Conditions;

namespace Innoactive.Hub.Training
{
    public class BaseAutocompleter<TData> : IAutocompleter<TData> where TData : ICompletableData
    {
        public virtual void Complete(TData data)
        {
            data.IsCompleted = true;
        }
    }
}