using System;

namespace Innoactive.Hub.Training.SceneObjects
{
    public class LockStateChangedEventArgs : EventArgs
    {
        public readonly bool IsLocked;

        public LockStateChangedEventArgs(bool isLocked)
        {
            IsLocked = isLocked;
        }
    }
}
