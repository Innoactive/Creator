namespace Innoactive.Creator.Core.Input
{
    /// <summary>
    /// Allows to prioritize input actions.
    /// </summary>
    public interface IInputActionListener
    {
        /// <summary>
        /// Priority of this input.
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// If this listener ignores a set focus, it will also be called when focus is active.
        /// </summary>
        bool IgnoreFocus { get; }
    }
}
