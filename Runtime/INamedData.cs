using System.Runtime.Serialization;
using VPG.Creator.Core.Attributes;

namespace VPG.Creator.Core
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
