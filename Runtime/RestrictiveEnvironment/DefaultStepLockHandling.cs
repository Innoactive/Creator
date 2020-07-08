using System.Collections.Generic;
using System.Linq;
using Innoactive.Creator.Core.Configuration.Modes;
using Innoactive.Creator.Core.Properties;
using Innoactive.Creator.Unity;

namespace Innoactive.Creator.Core.RestrictiveEnvironment
{
    /// <summary>
    /// Restricts interaction with scene objects by using LockableProperties, which are extracted from the <see cref="IStepData"/>.
    /// </summary>
    public class DefaultStepLockHandling : StepLockHandlingStrategy
    {
        private bool lockOnCourseStart = true;
        private bool lockOnCourseFinished = true;

        /// <inheritdoc />
        public override void Unlock(IStepData data, IEnumerable<LockablePropertyData> manualUnlocked)
        {
            IEnumerable<LockablePropertyData> unlockList = PropertyReflectionHelper.ExtractLockablesFromStep(data);
            unlockList = unlockList.Union(manualUnlocked);

            foreach (LockablePropertyData lockable in unlockList)
            {
                lockable.Property.SetLocked(false);
            }
        }

        /// <inheritdoc />
        public override void Lock(IStepData data, IEnumerable<LockablePropertyData> manualUnlocked)
        {
            // All properties which should be locked
            IEnumerable<LockablePropertyData> lockList = PropertyReflectionHelper.ExtractLockablesFromStep(data);
            lockList = lockList.Union(manualUnlocked);

            ITransition completedTransition = data.Transitions.Data.Transitions.FirstOrDefault(transition => transition.IsCompleted);
            if (completedTransition != null && completedTransition.Data.TargetStep != null)
            {
                IEnumerable<LockablePropertyData> nextStepProperties = PropertyReflectionHelper.ExtractLockablesFromStep(completedTransition.Data.TargetStep.Data);

                if (completedTransition.Data.TargetStep.Data is ILockableStepData lockableStepData)
                {
                    IEnumerable<LockablePropertyData> toUnlock = lockableStepData.ToUnlock.Select(reference => new LockablePropertyData(reference.GetProperty()));
                    nextStepProperties = nextStepProperties.Union(toUnlock);
                }

                if (completedTransition is ILockablePropertiesProvider completedLockableTransition)
                {
                    IEnumerable<LockablePropertyData> transitionLockList = completedLockableTransition.GetLockableProperties();
                    foreach (LockablePropertyData lockable in transitionLockList)
                    {
                        lockable.Property.SetLocked(lockable.EndStepLocked && nextStepProperties.Contains(lockable) == false);
                    }

                    // Remove all lockable from completed transition
                    lockList = lockList.Except(transitionLockList);
                }
                // Remove all lockable from
                lockList = lockList.Except(nextStepProperties);
            }

            foreach (LockablePropertyData lockable in lockList)
            {
                lockable.Property.SetLocked(true);
            }
        }

        /// <inheritdoc />
        public override void Configure(IMode mode)
        {
            if (mode.ContainsParameter<bool>("LockOnCourseStart"))
            {
                lockOnCourseStart = mode.GetParameter<bool>("LockOnCourseStart");
            }

            if (mode.ContainsParameter<bool>("LockOnCourseFinished"))
            {
                lockOnCourseFinished = mode.GetParameter<bool>("LockOnCourseFinished");
            }
        }

        /// <inheritdoc />
        public override void OnCourseStarted(ICourse course)
        {
            if (lockOnCourseStart)
            {
                foreach (LockableProperty prop in SceneUtils.GetActiveAndInactiveComponents<LockableProperty>())
                {
                    prop.SetLocked(true);
                }
            }
        }

        /// <inheritdoc />
        public override void OnCourseFinished(ICourse course)
        {
            if (lockOnCourseFinished)
            {
                foreach (LockableProperty prop in SceneUtils.GetActiveAndInactiveComponents<LockableProperty>())
                {
                    prop.SetLocked(true);
                }
            }
        }
    }
}
