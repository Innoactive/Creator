using System;
using Innoactive.CreatorEditor.UI.Windows;

namespace Innoactive.CreatorEditor.UndoRedo
{
    /// <summary>
    /// A <see cref="CallbackCommand"/> which marks <see cref="TrainingWindow"/> as dirty.
    /// </summary>
    public class TrainingCommand : CallbackCommand
    {
        private bool wasDirty;

        /// <inheritdoc />
        public override void Do()
        {
            if (TrainingWindow.IsOpen)
            {
                wasDirty = TrainingWindow.GetWindow().IsDirty;
                TrainingWindow.GetWindow().IsDirty = true;
            }

            base.Do();
        }

        /// <inheritdoc />
        public override void Undo()
        {
            base.Undo();

            if (TrainingWindow.IsOpen)
            {
                TrainingWindow.GetWindow().IsDirty = wasDirty;
            }
        }

        public TrainingCommand(Action doCallback, Action undoCallback) : base(doCallback, undoCallback) { }
    }
}
