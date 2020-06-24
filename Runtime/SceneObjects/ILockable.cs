﻿using System;

namespace Innoactive.Creator.Core.SceneObjects
{
    /// <summary>
    /// Basic interface for everything which is lockable.
    /// </summary>
    public interface ILockable
    {
        event EventHandler<LockStateChangedEventArgs> Locked;
        event EventHandler<LockStateChangedEventArgs> Unlocked;

        /// <summary>
        /// Returns if the object is locked.
        /// </summary>
        bool IsLocked { get; }

        /// <summary>
        /// Changes the lock state of the object.
        /// </summary>
        void SetLocked(bool lockState);
    }
}
