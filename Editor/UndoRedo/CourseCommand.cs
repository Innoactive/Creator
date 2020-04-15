using System;
using Innoactive.CreatorEditor.UI.Windows;

namespace Innoactive.CreatorEditor.UndoRedo
{
    /// <summary>
    /// A <see cref="CallbackCommand"/> which makes the <seealso cref="CourseAssetManager"/> to save the course.
    /// </summary>
    public class CourseCommand : CallbackCommand
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

        public CourseCommand(Action doCallback, Action undoCallback) : base(doCallback, undoCallback)
        {
        }
    }
}
