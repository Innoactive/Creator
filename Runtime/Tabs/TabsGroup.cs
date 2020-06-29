using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Innoactive.Creator.Core.Tabs
{
    /// <inheritdoc cref="ITabsGroup"/>
    [DataContract]
    internal class TabsGroup : ITabsGroup
    {
        /// <inheritdoc />
        [DataMember]
        public int Selected { get; set; }

        /// <inheritdoc />
        public IList<ITab> Tabs { get; }

        public TabsGroup(params ITab[] tabs)
        {
            Tabs = tabs;
        }
    }
}
