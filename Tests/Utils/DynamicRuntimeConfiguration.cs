using System.Collections.Generic;
using System.Linq;
using VPG.Core.Configuration;
using VPG.Core.Configuration.Modes;

namespace VPG.Tests.Utils
{
    /// <summary>
    /// DynamicDefinition allows to dynamically adjust training modes.
    /// </summary>
    public class DynamicRuntimeConfiguration : DefaultRuntimeConfiguration
    {
        public DynamicRuntimeConfiguration()
        {
            Modes = new BaseModeHandler(new List<IMode> {DefaultMode});
        }

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
