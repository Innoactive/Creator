namespace Innoactive.Hub.Training
{
    public interface IProcess<in TData> where TData : IData
    {
        IStageProcess<TData> GetStageProcess(Stage stage);
    }
}
