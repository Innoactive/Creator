using System.Runtime.Serialization;

namespace Innoactive.Creator.Core
{
    public interface IData
    {
        [DataMember]
        Metadata Metadata { get; set; }
    }
}
