using System.Collections.Generic;
using Innoactive.Creator.Core.RestrictiveEnvironment;

namespace Innoactive.Creator.Core
{
    public interface ILockableTransition
    {
        IEnumerable<LockablePropertyReference> GetLockableProperties();
    }
}
