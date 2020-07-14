using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Innoactive.Creator.Core.Tabs
{
    /// <summary>
    /// Draws a view with multiple tabs.
    /// </summary>
    internal interface ITabsGroup
    {
        /// <summary>
        /// Index of the currently selected tab.
        /// </summary>
        [DataMember]
        int Selected { get; set; }

        /// <summary>
        /// Tabs to display. See <seealso cref="ITab"/>.
        /// </summary>
        IList<ITab> Tabs { get; }
    }
}
