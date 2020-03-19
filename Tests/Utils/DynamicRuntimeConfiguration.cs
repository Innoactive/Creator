using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Innoactive.Creator.Core.Configuration;
using Innoactive.Creator.Core.Configuration.Modes;

namespace Innoactive.Creator.Tests.Utils
{
    /// <summary>
    /// DynamicDefinition allows to dynamically adjust training modes.
    /// </summary>
    public class DynamicRuntimeConfiguration : DefaultRuntimeConfiguration
    {
        private List<IMode> availableModes = null;

        public override ReadOnlyCollection<IMode> AvailableModes
        {
            get
            {
                if (availableModes == null)
                {
                    return base.AvailableModes;
                }

                return new ReadOnlyCollection<IMode>(availableModes);
            }
        }

        public void SetAvailableModes(IList<IMode> modes)
        {
            availableModes = modes.ToList();
        }

        public DynamicRuntimeConfiguration(params IMode[] modes)
        {
            if (modes.Length > 0)
            {
                availableModes = modes.ToList();
            }
        }
    }
}
