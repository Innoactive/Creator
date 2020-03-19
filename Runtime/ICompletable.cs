namespace Innoactive.Creator.Core
{
    public interface ICompletable
    {
        bool IsCompleted { get; }
        void Autocomplete();
    }
}
