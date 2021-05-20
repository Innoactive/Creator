﻿using System;
 using VPG.Core.SceneObjects;
 using UnityEngine;

namespace VPG.Core.Properties
{
    /// <summary>
    /// <see cref="TrainingSceneObjectProperty"/> which is lockable, to allow the restrictive environment to handle
    /// locking/unlocking your properties, extend this class.
    /// </summary>
    public abstract class LockableProperty : TrainingSceneObjectProperty, ILockable
    {
        ///  <inheritdoc/>
        public event EventHandler<LockStateChangedEventArgs> Locked;
        ///  <inheritdoc/>
        public event EventHandler<LockStateChangedEventArgs> Unlocked;

        [SerializeField]
        private bool lockOnParentObjectLock = true;

        /// <summary>
        /// Decides if the property will be locked when the parent scene object is locked.
        /// </summary>
        public bool LockOnParentObjectLock
        {
            get => lockOnParentObjectLock;
            set => lockOnParentObjectLock = value;
        }

        /// <inheritdoc/>
        public bool IsLocked { get; private set; }

        /// <summary>
        /// On default the lockable property will use this value to determine if its locked at the end of a step.
        /// </summary>
        public virtual bool EndStepLocked { get; } = true;

        protected override void OnEnable()
        {
            base.OnEnable();

            SceneObject.Locked += HandleObjectLocked;
            SceneObject.Unlocked += HandleObjectUnlocked;
        }

        protected virtual void OnDisable()
        {
            SceneObject.Locked -= HandleObjectLocked;
            SceneObject.Unlocked -= HandleObjectUnlocked;
        }

        /// <inheritdoc/>
        public virtual void SetLocked(bool lockState)
        {
            if (IsLocked == lockState)
            {
                return;
            }

            IsLocked = lockState;

            InternalSetLocked(lockState);

            if (IsLocked)
            {
                if (Locked != null)
                {
                    Locked.Invoke(this, new LockStateChangedEventArgs(IsLocked));
                }
            }
            else
            {
                if (Unlocked != null)
                {
                    Unlocked.Invoke(this, new LockStateChangedEventArgs(IsLocked));
                }
            }
        }

        private void HandleObjectUnlocked(object sender, LockStateChangedEventArgs e)
        {
            if (LockOnParentObjectLock && IsLocked)
            {
                SetLocked(false);
            }
        }

        private void HandleObjectLocked(object sender, LockStateChangedEventArgs e)
        {
            if (LockOnParentObjectLock && IsLocked == false)
            {
                SetLocked(true);
            }
        }

        /// <summary>
        /// Handle your internal locking affairs here.
        /// </summary>
        protected abstract void InternalSetLocked(bool lockState);
    }
}
