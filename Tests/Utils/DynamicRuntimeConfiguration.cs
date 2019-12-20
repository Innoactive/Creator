using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Innoactive.Hub.Training.Configuration;
using Innoactive.Hub.Training.Configuration.Modes;

namespace Innoactive.Hub.Unity.Tests.Training.Utils
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
