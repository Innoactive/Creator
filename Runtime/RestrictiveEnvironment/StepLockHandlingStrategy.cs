using System.Collections.Generic;
using VPG.Creator.Core.Configuration.Modes;

namespace VPG.Creator.Core.RestrictiveEnvironment
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

        /// <summary>
        /// Will be called whenever the mode is changed, allows to adapt to lock handling to it.
        /// </summary>
        public virtual void Configure(IMode mode)
        {

        }

        /// <summary>
        /// Will be called once when a course is started.
        /// </summary>
        public virtual void OnCourseStarted(ICourse course)
        {

        }

        /// <summary>
        /// Will be called once when the currently running course finishes.
        /// </summary>
        public virtual void OnCourseFinished(ICourse course)
        {

        }
    }
}
