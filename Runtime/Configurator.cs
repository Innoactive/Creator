using VPG.Core.Configuration.Modes;

namespace VPG.Core
{
    /// <summary>
    /// A base class for entities' configurators which have access to their entities' data.
    /// </summary>
    public abstract class Configurator<TData> : IConfigurator where TData : IData
    {
        /// <summary>
        /// The data to configure.
        /// </summary>
        protected TData Data { get; }

        protected Configurator(TData data)
        {
            Data = data;
        }

        /// <inheritdoc />
        public abstract void Configure(IMode mode, Stage stage);
    }
}
