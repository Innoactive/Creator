using System;
using System.Collections.Generic;
using Innoactive.Creator.Core.Input;
using Innoactive.Creator.Unity;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Central unity instance for input via the new Input System using C# events.
/// </summary>
public class InputController : UnitySceneSingleton<InputController>
{
    private struct ListenerInfo
    {
        public readonly IInputActionListener ActionListener;
        public readonly Func<InputAction.CallbackContext, bool> Action;

        public ListenerInfo(IInputActionListener actionListener, Func<InputAction.CallbackContext, bool> action)
        {
            ActionListener = actionListener;
            Action = action;
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

    private readonly Dictionary<string, List<ListenerInfo>> listenerDictionary = new Dictionary<string, List<ListenerInfo>>();

    private string defaultActionMap;

    private PlayerInput playerInput;

    /// <summary>
    /// Registers an action event to input.
    /// </summary>
    /// <param name="listener">The listener owning the action.</param>
    /// <param name="action">The action method which will be called.</param>
    public void RegisterEvent(IInputActionListener listener, Func<InputAction.CallbackContext, bool> action)
    {
        string actionName = action.Method.Name;
        if (listenerDictionary.ContainsKey(actionName) == false)
        {
            listenerDictionary.Add(actionName, new List<ListenerInfo>());
        }

        List<ListenerInfo> infoList = listenerDictionary[actionName];

        infoList.Add(new ListenerInfo(listener, action));
        infoList.Sort((l1, l2) => l1.ActionListener.Priority.CompareTo(l2.ActionListener.Priority) * -1);
    }

    /// <summary>
    /// Unregisters the given listeners action.
    /// </summary>
    public void UnregisterEvent(IInputActionListener listener, Func<InputAction.CallbackContext, bool> action)
    {
        string actionName = action.Method.Name;
        List<ListenerInfo> infoList = listenerDictionary[actionName];
        infoList.RemoveAll(info => info.ActionListener == listener && info.Action.Method.Name == actionName);
    }

    /// <summary>
    /// Focus the given input focus target.
    /// </summary>
    public void Focus(IInputFocus target)
    {
        if (target == CurrentInputFocus)
        {
            return;
        }

        CurrentInputFocus = target;
        if (string.IsNullOrEmpty(target.ActionMapName) == false)
        {
            playerInput.SwitchCurrentActionMap(target.ActionMapName);
        }

        target.OnFocus();
        OnFocused?.Invoke(this, new InputFocusEventArgs(target));
    }

    /// <summary>
    /// Releases the focus, if possible.
    /// </summary>
    public void ReleaseFocus()
    {
        if (CurrentInputFocus != null)
        {
            CurrentInputFocus.OnReleaseFocus();

            CurrentInputFocus = null;
            playerInput.SwitchCurrentActionMap(defaultActionMap);

            OnFocusReleased?.Invoke(this, new InputFocusEventArgs(null));
        }
    }

    protected override void Awake()
    {
        base.Awake();
        playerInput = GetComponent<PlayerInput>();
    }

    protected void OnEnable()
    {
        playerInput.onActionTriggered += OnActionTriggered;
        defaultActionMap = playerInput.defaultActionMap;
    }

    protected void OnDisable()
    {
        playerInput.onActionTriggered -= OnActionTriggered;
    }

    /// <summary>
    /// Internal method handling all actions triggered by the new input system.
    /// </summary>
    protected virtual void OnActionTriggered(InputAction.CallbackContext context)
    {
        if (context.action.triggered == false || listenerDictionary.ContainsKey(context.action.name) == false)
        {
            return;
        }

        List<ListenerInfo> infoList = listenerDictionary[context.action.name];

        foreach (ListenerInfo info in infoList)
        {
            try
            {
                if (CurrentInputFocus != null && info.ActionListener.IgnoreFocus == false && info.ActionListener != CurrentInputFocus)
                {
                    break;
                }

                if (info.Action(context))
                {
                    break;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }
    }
}
