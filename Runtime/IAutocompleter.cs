using Innoactive.Hub.Training.Conditions;

namespace Innoactive.Hub.Training
{
    public interface IAutocompleter<in TData> where TData : ICompletableData
    {
        void Complete(TData data);
    }
}