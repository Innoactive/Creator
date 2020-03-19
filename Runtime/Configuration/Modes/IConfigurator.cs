namespace Innoactive.Creator.Core.Configuration.Modes
{
    public interface IConfigurator<in TData> where TData : IData
    {
        void Configure(TData data, IMode mode, Stage stage);
    }
}
