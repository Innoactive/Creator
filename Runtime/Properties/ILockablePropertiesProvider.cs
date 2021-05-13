using System.Collections.Generic;
using VPG.Creator.Core.RestrictiveEnvironment;

namespace VPG.Creator.Core
{
    /// <summary>
    /// This interface is used to allow entities, for example <see cref="Transition"/> or <see cref="Conditions"/>
    /// to provide properties which should be locked.
    /// </summary>
    public interface ILockablePropertiesProvider
    {
        /// <summary>
        /// Returns all LockableProperties this provider requires.
        /// </summary>
        IEnumerable<LockablePropertyData> GetLockableProperties();
    }
}
