using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Innoactive.Creator.Core.Tabs
{
    internal interface ITabsGroup
    {
        [DataMember]
        int Selected { get; set; }
        IList<ITab> Tabs { get; }
    }
}
