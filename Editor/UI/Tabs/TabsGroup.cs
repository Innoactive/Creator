using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Innoactive.Creator.Core;
using UnityEngine;

namespace Innoactive.CreatorEditor.Tabs
{
    /// <inheritdoc cref="ITabsGroup"/>
    [DataContract]
    internal class TabsGroup : ITabsGroup
    {
        public const string SelectedKey = "selected";

        Metadata ParentMetadata { get; }

        /// <inheritdoc />
        public int Selected
        {
            get
            {
                var value = ParentMetadata.GetMetadata(GetType())[SelectedKey];
                return Convert.ToInt32(value);
            }

            set
            {
                ParentMetadata.SetMetadata(GetType(), SelectedKey, value);
            }
        }

        /// <inheritdoc />
        public IList<ITab> Tabs { get; }

        public TabsGroup(Metadata parentMetadata, params ITab[] tabs)
        {
            ParentMetadata = parentMetadata;
            if (ParentMetadata.GetMetadata(GetType()).ContainsKey(SelectedKey) == false)
            {
                Selected = 0;
            }
            Tabs = tabs;
        }
    }
}
