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
            base.Do();
            CourseAssetManager.Save();
        }

        /// <inheritdoc />
        public override void Undo()
        {
            base.Undo();
            CourseAssetManager.Save();
        }

        public TrainingCommand(Action doCallback, Action undoCallback) : base(doCallback, undoCallback)
        {
        }
    }
}