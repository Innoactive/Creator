using System;
using System.Reflection;

namespace Innoactive.Creator.Core.Attributes
{
    /// <summary>
    /// Declares that the "Is Blocking" toggle has to be drawn, if the behavior implements <see cref="IBackgroundBehaviorData"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DrawIsBlockingToggleAttribute : MetadataAttribute
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
