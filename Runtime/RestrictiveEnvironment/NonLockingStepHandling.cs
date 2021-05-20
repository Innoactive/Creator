using System.Collections.Generic;
using System.Linq;

namespace VPG.Core.RestrictiveEnvironment
{
    /// <summary>
    /// This implementation does not care about restrictive environment and does nothing.
    /// Use this strategy to disable the feature.
    /// </summary>
    public class NonLockingStepHandling : StepLockHandlingStrategy
    {
        /// <inheritdoc />
        public override void Unlock(IStepData data, IEnumerable<LockablePropertyData> manualUnlocked)
        {

        }

        /// <inheritdoc />
        public override void Lock(IStepData data, IEnumerable<LockablePropertyData> manualUnlocked)
        {

        }
    }
}
