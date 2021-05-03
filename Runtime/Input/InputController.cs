using System;
using System.Collections.Generic;
using Innoactive.Creator.Unity;

namespace Innoactive.Creator.Core.Input
{
    /// <summary>
    /// Central controller for input via the new Input System using C# events.
    /// </summary>
    public abstract class InputController : UnitySceneSingleton<InputController>
    {
        public class InputEventArgs : EventArgs
        {
            public readonly object Context;

            public InputEventArgs(object context)
            {
                Context = context;
            }
        }

        public class InputFocusEventArgs : EventArgs
        {
            public readonly IInputFocus InputFocus;

            public InputFocusEventArgs(IInputFocus inputFocus)
            {
                InputFocus = inputFocus;
            }
        }

        protected struct ListenerInfo
        {
            public readonly IInputActionListener ActionListener;
            public readonly Action<InputEventArgs> Action;

            public ListenerInfo(IInputActionListener actionListener, Action<InputEventArgs> action)
            {
                ActionListener = actionListener;
                Action = action;
            }
        }

        /// <summary>
        /// Will be called when an object is focused.
        /// </summary>
        public EventHandler<InputFocusEventArgs> OnFocused;

        /// <summary>
        /// Will be called when the focus on an object is released.
        /// </summary>
        public EventHandler<InputFocusEventArgs> OnFocusReleased;

        /// <summary>
        /// Currently focused object.
        /// </summary>
        protected IInputFocus CurrentInputFocus { get; set; } = null;

        protected Dictionary<string, List<ListenerInfo>> ListenerDictionary { get; } = new Dictionary<string, List<ListenerInfo>>();

        /// <summary>
        /// Registers an action event to input.
        /// </summary>
        /// <param name="listener">The listener owning the action.</param>
        /// <param name="action">The action method which will be called.</param>
        public void RegisterEvent(IInputActionListener listener, Action<InputEventArgs> action)
        {
            string actionName = action.Method.Name;
            if (ListenerDictionary.ContainsKey(actionName) == false)
            {
                ListenerDictionary.Add(actionName, new List<ListenerInfo>());
            }

            List<ListenerInfo> infoList = ListenerDictionary[actionName];

            infoList.Add(new ListenerInfo(listener, action));
            infoList.Sort((l1, l2) => l1.ActionListener.Priority.CompareTo(l2.ActionListener.Priority) * -1);
        }

        /// <summary>
        /// Unregisters the given listeners action.
        /// </summary>
        public void UnregisterEvent(IInputActionListener listener, Action<InputEventArgs> action)
        {
            string actionName = action.Method.Name;
            List<ListenerInfo> infoList = ListenerDictionary[actionName];
            infoList.RemoveAll(info => info.ActionListener == listener && info.Action.Method.Name == actionName);
        }

        /// <summary>
        /// Focus the given input focus target.
        /// </summary>
        public abstract void Focus(IInputFocus target);

        public abstract void ReleaseFocus();

        protected override void Awake()
        {
            base.Awake();
            Setup();
        }

        protected void Reset()
        {
            Setup();
        }


        protected abstract void Setup();
    }
}
