using System;

namespace VPG.Creator.Core.SceneObjects
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
