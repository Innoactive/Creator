using System.Runtime.Serialization;
using UnityEngine;

namespace Innoactive.Creator.Core
{
    [DataContract(IsReference = true)]
    public class ChapterMetadata : IMetadata
    {
        [DataMember]
        public IStep LastSelectedStep { get; set; }

        [DataMember]
        public Vector2 EntryNodePosition { get; set; }

        public ChapterMetadata()
        {
        }
    }
}
