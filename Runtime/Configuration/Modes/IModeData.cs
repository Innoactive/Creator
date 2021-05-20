namespace VPG.Core.Configuration.Modes
{
    public interface IModeData : IData
    {
        IMode Mode { get; set; }
    }
}
