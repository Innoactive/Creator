using System;

namespace Innoactive.Creator.Core.Attributes
{
    /// <summary>
    /// Add a link to a documentation which explains a behavior or condition.
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
