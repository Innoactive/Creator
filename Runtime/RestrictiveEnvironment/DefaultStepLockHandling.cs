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
        /// <summary>
        /// If you want to disable lock handling set this parameter to false.
        /// </summary>
        public const string EnableLockHandlingParameterName = "EnableLockHandling";

        /// <summary>
        /// To stop locking every LockableProperty at the beginning of the training set this parameter to false.
        /// </summary>
        public const string LockCourseOnStartParameterName = "LockCourseOnStart";

        /// <summary>
        /// To lock every LockableProperty at the end of the training set this parameter to true.
        /// </summary>
        public const string LockOnCourseFinishedParameterName = "LockOnCourseFinished";

        private bool lockOnCourseStart = true;
        private bool lockOnCourseFinished = true;
        private bool isEnabled = true;

        /// <inheritdoc />
        public override void Unlock(IStepData data, IEnumerable<LockablePropertyData> manualUnlocked)
        {
            if (isEnabled == false)
            {
                return;
            }

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
            if (isEnabled == false)
            {
                return;
            }

            // All properties which should be locked
            IEnumerable<LockablePropertyData> lockList = PropertyReflectionHelper.ExtractLockablePropertiesFromStep(data);
            lockList = lockList.Union(manualUnlocked);

            ITransition completedTransition = data.Transitions.Data.Transitions.FirstOrDefault(transition => transition.IsCompleted);
            if (completedTransition != null)
            {
                IStepData nextStepData = GetNextStep(completedTransition);
                IEnumerable<LockablePropertyData> nextStepProperties = PropertyReflectionHelper.ExtractLockablePropertiesFromStep(nextStepData);
                if (nextStepData != null && nextStepData is ILockableStepData lockableStepData)
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

        private IStepData GetNextStep(ITransition completedTransition)
        {
            if (completedTransition.Data.TargetStep != null)
            {
                return completedTransition.Data.TargetStep.Data;
            }

            if (CourseRunner.IsRunning)
            {
                ICourseData course = CourseRunner.Current.Data;
                // Test all chapters, but the last.
                for (int i = 0; i < course.Chapters.Count - 1; i++)
                {
                    if (course.Chapters[i] == course.Current)
                    {
                        return course.Chapters[i + 1].Data.FirstStep.Data;
                    }
                }
            }
            // No next step found, seems to be the last.
            return null;
        }

        /// <inheritdoc />
        public override void Configure(IMode mode)
        {
            if (mode.ContainsParameter<bool>(EnableLockHandlingParameterName))
            {
                isEnabled = mode.GetParameter<bool>(EnableLockHandlingParameterName);
            }

            if (mode.ContainsParameter<bool>(LockCourseOnStartParameterName))
            {
                lockOnCourseStart = mode.GetParameter<bool>(LockCourseOnStartParameterName);
            }

            if (mode.ContainsParameter<bool>(LockOnCourseFinishedParameterName))
            {
                lockOnCourseFinished = mode.GetParameter<bool>(LockOnCourseFinishedParameterName);
            }
        }

        /// <inheritdoc />
        public override void OnCourseStarted(ICourse course)
        {
            if (isEnabled && lockOnCourseStart)
            {
                EnforceAllProperties(true);
            }
        }

        /// <inheritdoc />
        public override void OnCourseFinished(ICourse course)
        {
            if (isEnabled && lockOnCourseFinished)
            {
                EnforceAllProperties(true);
            }
        }

        private void EnforceAllProperties(bool isLocked)
        {
            foreach (LockableProperty prop in SceneUtils.GetActiveAndInactiveComponents<LockableProperty>())
            {
                prop.SetLocked(isLocked);
            }
        }
    }
}
