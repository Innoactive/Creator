using System.Runtime.Serialization;

namespace Innoactive.Hub.Training.Behaviors
{
    [DataContract(IsReference = true)]
    public class BehaviorMetadata : IMetadata
    {
        [DataMember]
        public bool IsFoldedOut { get; set; }
    }
}
