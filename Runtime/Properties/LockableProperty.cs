﻿using System;
 using Innoactive.Creator.Core.SceneObjects;
 using UnityEngine;

namespace Innoactive.Creator.Core.Properties
{
    public abstract class LockableProperty : TrainingSceneObjectProperty, ILockable
    {
        public event EventHandler<LockStateChangedEventArgs> Locked;
        public event EventHandler<LockStateChangedEventArgs> Unlocked;

        [SerializeField]
        private bool lockOnParentObjectLock = true;
        
        public bool LockOnParentObjectLock
        {
            get
            {
                return lockOnParentObjectLock;
            }
            set
            {
                lockOnParentObjectLock = value;
            }
        }

        public bool IsLocked { get; private set; }

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

        protected abstract void InternalSetLocked(bool lockState);
    }
}
