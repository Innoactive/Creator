namespace Innoactive.Hub.Training
{
    public interface ICompletable
    {
        bool IsCompleted { get; }
        void Autocomplete();
    }
}
