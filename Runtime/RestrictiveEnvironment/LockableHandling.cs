using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Innoactive.Creator.Core.RestrictiveEnvironment
{
    internal static class LockableHandling
    {
        public static void UnlockPropertiesForStepData(IStepData data)
        {
            IEnumerable<LockablePropertyReference> unlockList = GetLockablePropertiesFrom(data);
            foreach (LockablePropertyReference lockable in unlockList)
            {
                Debug.Log("unlock: " + lockable.Property.GetType());
                lockable.Property.SetLocked(false);
            }
        }

        public static void LockPropertiesForStepData(IStepData data)
        {
            // All properties which should be locked
            IEnumerable<LockablePropertyReference> lockList = GetLockablePropertiesFrom(data);

            ITransition completedTransition = data.Transitions.Data.Transitions.First(transition => transition.IsCompleted);
            if (completedTransition != null)
            {
                IEnumerable<LockablePropertyReference> nextStepProperties = GetLockablePropertiesFrom(completedTransition.Data.TargetStep.Data);
                if (completedTransition is ILockableTransition completedLockableTransition)
                {
                    IEnumerable<LockablePropertyReference> transitionLockList = completedLockableTransition.GetLockableProperties();
                    foreach (LockablePropertyReference lockable in transitionLockList)
                    {
                        lockable.Property.SetLocked(lockable.EndStepLocked && nextStepProperties.Contains(lockable) == false);
                    }

                    // Remove all lockable from completed transition
                    lockList = lockList.Except(transitionLockList);
                }
                // Remove all lockable from
                lockList = lockList.Except(nextStepProperties);
            }

            foreach (LockablePropertyReference lockable in lockList)
            {
                Debug.Log("lock: " + lockable.Property.GetType());
                lockable.Property.SetLocked(true);
            }
        }

        private static IEnumerable<LockablePropertyReference> GetLockablePropertiesFrom(IStepData data)
        {
            IEnumerable<LockablePropertyReference> result = new List<LockablePropertyReference>();
            foreach (ITransition transition in data.Transitions.Data.Transitions)
            {
                if (transition.IsCompleted == false && transition is ILockableTransition lockableTransition)
                {
                    result = result.Union(lockableTransition.GetLockableProperties());
                }
            }

            return result;
        }
    }
}
