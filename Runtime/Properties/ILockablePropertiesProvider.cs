using System.Collections.Generic;
using Innoactive.Creator.Core.RestrictiveEnvironment;

namespace Innoactive.Creator.Core
{
    /// <summary>
    /// This interface is used to allow entities, for example <see cref="Transition"/> or <see cref="Conditions"/>
    /// to provide properties which should be locked.
    /// </summary>
    public interface ILockablePropertiesProvider
    {
        IEnumerable<LockablePropertyData> GetLockableProperties();
    }
}
