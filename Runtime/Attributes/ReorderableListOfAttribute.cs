using System;

namespace VPG.Core.Attributes
{
    /// <summary>
    /// Declares that children of this list have metadata attributes and can be reordered.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ReorderableListOfAttribute : ListOfAttribute
    {
        /// <inheritdoc />
        public ReorderableListOfAttribute(params Type[] childAttributes) : base(childAttributes) { }
    }
}
