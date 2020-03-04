namespace Innoactive.Hub.Training.Configuration.Modes
{
    public interface IModeData : IData
    {
        IMode Mode { get; set; }
    }
}
