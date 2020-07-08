using System.Collections.Generic;

namespace Innoactive.Creator.Core.RestrictiveEnvironment
{
    /// <summary>
    /// Allows to implement strategies which restrict interaction with scene objects for specific steps.
    /// </summary>
    public abstract class StepLockHandlingStrategy
    {
        /// <summary>
        /// Unlocks the restrictive environment and allows interaction with scene objects required for this Step to complete.
        /// </summary>
        /// <param name="data">IStepData of the current step</param>
        /// <param name="manualUnlocked">All LockableProperties which are should be unlocked in addition</param>
        public abstract void Unlock(IStepData data, IEnumerable<LockablePropertyData> manualUnlocked);

        /// <summary>
        /// Locks all unlocked LockableProperties for the current step.
        /// </summary>
        /// <param name="data">IStepData of the current step</param>
        /// <param name="manualUnlocked">All LockableProperties which were unlocked in addition</param>
        public abstract void Lock(IStepData data, IEnumerable<LockablePropertyData> manualUnlocked);
    }
}
