using System;
using System.Reflection;

namespace VPG.Core.Attributes
{
    /// <summary>
    /// Declares that "Delete" button has to be drawn.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class KeepPopulatedAttribute : MetadataAttribute
    {
        /// <summary>
        /// Defines the type of an element to create.
        /// </summary>
        private readonly Type defaultType;

        public KeepPopulatedAttribute(Type type)
        {
            defaultType = type;
        }

        /// <inheritdoc />
        public override object GetDefaultMetadata(MemberInfo owner)
        {
            return defaultType;
        }

        /// <inheritdoc />
        public override bool IsMetadataValid(object metadata)
        {
            return metadata is Type;
        }
    }
}
