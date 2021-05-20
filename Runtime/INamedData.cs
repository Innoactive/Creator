using System.Runtime.Serialization;
using VPG.Core.Attributes;

namespace VPG.Core
{
    /// <summary>
    /// Data structure with an <see cref="IStep"/>'s name.
    /// </summary>
    public interface INamedData : IData
    {
        /// <summary>
        /// <see cref="IStep"/>'s name.
        /// </summary>
        [DataMember]
        [HideInTrainingInspector]
        string Name { get; set; }
    }
}
