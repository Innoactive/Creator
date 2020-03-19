using Innoactive.Creator.Core.Conditions;

namespace Innoactive.Creator.Core
{
    public interface IAutocompleter<in TData> where TData : ICompletableData
    {
        void Complete(TData data);
    }
}
