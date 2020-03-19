namespace Innoactive.Creator.Core
{
    public interface IProcess<in TData> where TData : IData
    {
        IStageProcess<TData> GetStageProcess(Stage stage);
    }
}
