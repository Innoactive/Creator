using System;

namespace VPG.Creator.Core.Attributes
{
    /// <summary>
    /// Adds a link to a documentation that explains a behavior or condition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class)]
    public class HelpLinkAttribute : Attribute
    {
        /// <summary>
        /// An HTML link to the documentation explaining the behavior or condition.
        /// </summary>
        public string HelpLink { get; private set; }

        public HelpLinkAttribute(string helpLink)
        {
            HelpLink = helpLink;
        }
    }
}
