namespace Innoactive.Creator.Core.Conditions
{
    public interface ICompletableData : IData
    {
        bool IsCompleted { get; set; }
    }
}
