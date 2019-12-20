﻿using System;

namespace Innoactive.Hub.Training.SceneObjects
{
    // TODO consider lockable-by-context, to allow multiple lockers to be active (e.g. both step and snap zone)
    public interface ILockable
    {
        event EventHandler<LockStateChangedEventArgs> Locked;
        event EventHandler<LockStateChangedEventArgs> Unlocked;

        bool IsLocked { get; }

        void SetLocked(bool lockState);
    }
}
