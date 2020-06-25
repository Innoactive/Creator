using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Innoactive.Creator.Core.Tabs
{
    [DataContract]
    internal class TabsGroup : ITabsGroup
    {
        [DataMember]
        public int Selected { get; set; }

        public IList<ITab> Tabs { get; }

        public TabsGroup(params ITab[] tabs)
        {
            Tabs = tabs;
        }
    }
}
