using System.Collections.Generic;
using VPG.Core.RestrictiveEnvironment;

namespace VPG.Core
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
