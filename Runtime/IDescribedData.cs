namespace Innoactive.Hub.Training
{
    public interface IDescribedData : IData
    {
        string Description { get; set; }
    }
}