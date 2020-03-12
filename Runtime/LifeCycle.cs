using System;
using System.Collections;
using System.Collections.Generic;
using Innoactive.Hub.Training.Exceptions;

namespace Innoactive.Hub.Training
{
    public sealed class LifeCycle : ILifeCycle
    {
        private bool deactivateAfterActivation;
        private IEnumerator update;

        private bool IsCurrentStageProcessFinished
        {
            get
            {
                return update == null;
            }
        }

        private readonly Dictionary<Stage, bool> fastForwardedStates = new Dictionary<Stage, bool>
        {
            { Stage.Inactive, false },
            { Stage.Activating, false },
            { Stage.Active, false },
            { Stage.Deactivating, false }
        };

        public IEntity Owner { get; private set; }

        public LifeCycle(IEntity owner)
        {
            Stage = Stage.Inactive;
            Owner = owner;
        }

        public event EventHandler<ActivationStateChangedEventArgs> StageChanged;

        public Stage Stage { get; private set; }

        public void Activate()
        {
            if (Stage != Stage.Inactive)
            {
                throw new InvalidStateException("Training entity can only be activated when not running yet");
            }

            StartActivating();
        }

        public void Deactivate()
        {
            if (Stage == Stage.Activating)
            {
                // Deactivate is called while activation is still running - this is valid, but
                // the actual deactivation has to be delayed until the activation is finished.
                deactivateAfterActivation = true;
            }
            else if (Stage != Stage.Active)
            {
                throw new InvalidStateException("Training entity can only be deactivated when already running");
            }
            else
            {
                StartDeactivating();
            }
        }

        public void MarkToFastForward()
        {
            fastForwardedStates[Stage.Deactivating] = true;

            if (Stage == Stage.Deactivating)
            {
                FastForward();
                return;
            }

            fastForwardedStates[Stage.Active] = true;

            if (Stage == Stage.Active)
            {
                FastForward();
                return;
            }

            fastForwardedStates[Stage.Activating] = true;

            FastForward();
        }

        public void MarkToFastForwardStage(Stage stage)
        {
            if (stage == Stage.Inactive)
            {
                return;
            }

            fastForwardedStates[stage] = true;

            if (stage == Stage)
            {
                FastForward();
            }
        }

        public void Update()
        {
            if (IsCurrentStageProcessFinished)
            {
                return;
            }

            if (update.MoveNext() == false)
            {
                FinishCurrentState();
            }
        }

        private void FastForward()
        {
            if (IsCurrentStageProcessFinished)
            {
                return;
            }

            Owner.InvokeProcessFastForward();
            FinishCurrentState();
        }

        private void FinishCurrentState()
        {
            update = null;

            Owner.InvokeProcessEnd();

            fastForwardedStates[Stage] = false;

            switch (Stage)
            {
                case Stage.Inactive:
                    return;
                case Stage.Activating:
                    StartActive();
                    return;
                case Stage.Active:
                    return;
                case Stage.Deactivating:
                    StartInactive();
                    return;
            }
        }

        private void StartActivating()
        {
            deactivateAfterActivation = false;

            ChangeStage(Stage.Activating);

            if (IsInFastForward)
            {
                FastForward();
            }
        }

        private void StartActive()
        {
            ChangeStage(Stage.Active);

            if (IsInFastForward)
            {
                FastForward();
            }

            if (deactivateAfterActivation)
            {
                Deactivate();
            }
        }

        private void StartDeactivating()
        {
            ChangeStage(Stage.Deactivating);

            if (IsInFastForward)
            {
                FastForward();
            }
        }

        private void StartInactive()
        {
            ChangeStage(Stage.Inactive);
        }

        private bool IsInFastForward
        {
            get
            {
                return fastForwardedStates[Stage];
            }
        }

        private void ChangeStage(Stage stage)
        {
            // Interrupt and fast-forward the current stage process, if it had no time to iterate completely.
            FastForward();

            Stage = stage;
            Owner.InvokeProcessStart();
            update = Owner.InvokeProcessUpdate();

            if (StageChanged != null)
            {
                StageChanged.Invoke(this, new ActivationStateChangedEventArgs(stage));
            }
        }
    }
}
