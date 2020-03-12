using System.Runtime.Serialization;

namespace Innoactive.Hub.Training
{
    public interface IData
    {

        [DataMember]
        Metadata Metadata { get; set; }
    }
}
