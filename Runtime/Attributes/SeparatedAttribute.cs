using System;
using System.Reflection;

namespace Innoactive.Creator.Core.Attributes
{
    /// <summary>
    /// Declare that this element has to be separated with thin gray lines.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SeparatedAttribute : MetadataAttribute
    {
        /// <inheritdoc />
        public override object GetDefaultMetadata(MemberInfo owner)
        {
            return null;
        }

        /// <inheritdoc />
        public override bool IsMetadataValid(object metadata)
        {
            return metadata == null;
        }
    }
}
