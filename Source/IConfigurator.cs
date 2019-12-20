using Innoactive.Hub.Training.Configuration.Modes;

namespace Innoactive.Hub.Training
{
    public interface IConfigurator<in TData> where TData : IData
    {
        void Configure(TData data, IMode mode, Stage stage);
    }
}
