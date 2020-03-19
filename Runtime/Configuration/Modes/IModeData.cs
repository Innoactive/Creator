using Innoactive.Creator.Core;

namespace Innoactive.Creator.Core.Configuration.Modes
{
    public interface IModeData : IData
    {
        IMode Mode { get; set; }
    }
}
