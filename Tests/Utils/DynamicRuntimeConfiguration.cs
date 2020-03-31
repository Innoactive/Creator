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
        public void SetAvailableModes(IList<IMode> modes)
        {
            Modes = new BaseModeHandler(modes.ToList());
        }

        public DynamicRuntimeConfiguration(params IMode[] modes)
        {
            SetAvailableModes(modes.ToList());
        }
    }
}
