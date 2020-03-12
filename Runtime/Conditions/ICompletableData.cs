namespace Innoactive.Hub.Training.Conditions
{
    public interface ICompletableData : IData
    {
        bool IsCompleted { get; set; }
    }
}
