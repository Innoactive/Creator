using System.Collections.Generic;
using Innoactive.Creator.Core.RestrictiveEnvironment;

namespace Innoactive.Creator.Core.Conditions
{
    public interface ILockableCondition
    {
        IEnumerable<LockablePropertyData> GetLockableProperties();
    }
}
