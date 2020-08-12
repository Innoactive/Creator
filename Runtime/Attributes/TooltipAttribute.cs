using System.Reflection;

namespace Innoactive.Creator.Core.Attributes
{
    public class TooltipAttribute : MetadataAttribute
    {
        private readonly string tooltip;

        public TooltipAttribute(string tooltip)
        {
            this.tooltip = tooltip;
        }

        public override object GetDefaultMetadata(MemberInfo owner)
        {
            return tooltip;
        }

        public override bool IsMetadataValid(object metadata)
        {
            return metadata is string;
        }
    }
}
