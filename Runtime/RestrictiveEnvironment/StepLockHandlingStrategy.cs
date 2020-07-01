using System.Collections.Generic;
using Innoactive.Creator.Core.RestrictiveEnvironment;

namespace Innoactive.Creator.Core.RestrictiveEnvironment
{
    public abstract class StepLockHandlingStrategy
    {
        public abstract void Unlock(IStepData data, IEnumerable<LockablePropertyData> manualUnlocked);
        public abstract void Lock(IStepData data, IEnumerable<LockablePropertyData> manualUnlocked);
    }
}
