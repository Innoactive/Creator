using System.Runtime.Serialization;

namespace Innoactive.Creator.Core
{
    /// <summary>
    /// Abstract data structure.
    /// </summary>
    public interface IData
    {
        /// <summary>
        /// Reference to this object's <see cref="IMetadata"/>.
        /// </summary>
        [DataMember]
        Metadata Metadata { get; set; }
    }
}
