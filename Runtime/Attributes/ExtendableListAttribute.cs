using System;
using System.Reflection;
using System.Runtime.Serialization;
using VPG.Creator.Core.Utils;

namespace VPG.Creator.Core.Attributes
{
    /// <summary>
    /// Declares that new elements can be added to this list.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ExtendableListAttribute : MetadataAttribute
    {
        /// <summary>
        /// Holds a serialized reference to the metadata object's type.
        /// </summary>
        [DataContract(IsReference = true)]
        public class SerializedTypeWrapper
        {
            /// <summary>
            /// Metadata object's type.
            /// </summary>
            [DataMember]
            public Type Type { get; set; }
        }

        /// <summary>
        /// Defines the type of an element to create.
        /// </summary>
        public Type DeclaredElementType { get; set; }

        /// <inheritdoc />
        public override object GetDefaultMetadata(MemberInfo owner)
        {
            return new SerializedTypeWrapper
            {
                Type = ReflectionUtils.GetEntryType(ReflectionUtils.GetDeclaredTypeOfPropertyOrField(owner))
            };
        }

        /// <inheritdoc />
        public override bool IsMetadataValid(object metadata)
        {
            return metadata is SerializedTypeWrapper;
        }
    }
}
