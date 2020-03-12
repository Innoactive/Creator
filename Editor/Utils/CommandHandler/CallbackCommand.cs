using System;

namespace Innoactive.Hub.Training.Editors.Utils.Undo
{
    /// <summary>
    /// A revertable command which defines Do/Undo logic through callbacks.
    /// </summary>
    public class CallbackCommand : IRevertableCommand
    {
        private readonly Action doCallback;
        private readonly Action undoCallback;

        public CallbackCommand(Action doCallback, Action undoCallback)
        {
            this.doCallback = doCallback;
            this.undoCallback = undoCallback;
        }

        /// <inheritdoc />
        public virtual void Do()
        {
            doCallback.Invoke();
        }

        /// <inheritdoc />
        public virtual void Undo()
        {
            undoCallback.Invoke();
        }
    }
}
