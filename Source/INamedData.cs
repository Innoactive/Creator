using System.Runtime.Serialization;
using Innoactive.Hub.Training.Attributes;

namespace Innoactive.Hub.Training
{
    public interface INamedData : IData
    {
        [DataMember]
        [HideInTrainingInspector]
        string Name { get; set; }
    }
}
