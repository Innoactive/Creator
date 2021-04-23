using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Innoactive.Creator.Core.Input
{
    /// <summary>
    /// Base class for InputActionListener.
    /// </summary>
    public abstract class InputActionListener : MonoBehaviour, IInputActionListener
    {
        /// <inheritdoc/>
        public virtual int Priority { get; } = 1000;

        /// <inheritdoc/>
        public virtual bool IgnoreFocus { get; } = false;

        /// <summary>
        /// Registers the given method as input event, the name of the method will be the event name.
        /// </summary>
        protected virtual void RegisterInputEvent(Action<InputAction.CallbackContext> action)
        {
            InputController.Instance.RegisterEvent(this, action);
        }

        /// <summary>
        /// Unregisters the given method as input event, the name of the method will be the event name.
        /// </summary>
        protected virtual void UnregisterInputEvent(Action<InputAction.CallbackContext> action)
        {
            InputController.Instance.UnregisterEvent(this, action);
        }
    }
}
