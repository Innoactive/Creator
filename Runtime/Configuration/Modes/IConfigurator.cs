namespace VPG.Creator.Core.Configuration.Modes
{
    /// <summary>
    /// An interface for entities' configurators.
    /// </summary>
    public interface IConfigurator
    {
        /// <paramref name="mode">The current mode.</param>
        /// <paramref name="stage">The current entity's stage.</param>
        void Configure(IMode mode, Stage stage);
    }
}
