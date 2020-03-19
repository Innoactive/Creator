using System.Runtime.Serialization;
using Innoactive.Creator.Core.Attributes;

namespace Innoactive.Creator.Core
{
    public interface INamedData : IData
    {
        [DataMember]
        [HideInTrainingInspector]
        string Name { get; set; }
    }
}
