using System.Runtime.Serialization;
using UnityEngine;

namespace Innoactive.Creator.Core
{
    /// <summary>
    /// Implementation of <see cref="IMetadata"/> adapted for <see cref="IChapter"/> data.
    /// </summary>
    [DataContract(IsReference = true)]
    public class ChapterMetadata : IMetadata
    {
        /// <summary>
        /// Reference to last selected <see cref="IStep"/>.
        /// </summary>
        [DataMember]
        public IStep LastSelectedStep { get; set; }

        /// <summary>
        /// Reference to the entry node's position in the Workflow window.
        /// </summary>
        [DataMember]
        public Vector2 EntryNodePosition { get; set; }

        public ChapterMetadata()
        {
        }
    }
}
