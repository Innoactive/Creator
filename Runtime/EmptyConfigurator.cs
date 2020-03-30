using Innoactive.Creator.Core.Configuration.Modes;

namespace Innoactive.Creator.Core
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
