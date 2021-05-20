using VPG.Core.Configuration.Modes;

namespace VPG.Core
{
    /// <summary>
    /// A configurator that does nothing.
    /// </summary>
    public class EmptyConfigurator : IConfigurator
    {
        /// <inheritdoc />
        public void Configure(IMode mode, Stage stage)
        {
        }
    }
}
