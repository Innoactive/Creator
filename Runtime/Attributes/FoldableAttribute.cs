using System;
using System.Reflection;

namespace VPG.Creator.Core.Attributes
{
    /// <summary>
    /// Declares that this element's view has to be foldable.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FoldableAttribute : MetadataAttribute
    {
        /// <inheritdoc />
        public override object GetDefaultMetadata(MemberInfo owner)
        {
            return true;
        }

        /// <inheritdoc />
        public override bool IsMetadataValid(object metadata)
        {
            return metadata is bool;
        }
    }
}
