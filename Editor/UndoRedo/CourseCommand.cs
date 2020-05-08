using System;
using Innoactive.CreatorEditor.UI.Windows;

namespace Innoactive.CreatorEditor.UndoRedo
{
    /// <summary>
    /// A <see cref="CallbackCommand"/> which notifies the <seealso cref="GlobalEditorHandler"/> class that the current course was modified.
    /// </summary>
    public class CourseCommand : CallbackCommand
    {
        /// <inheritdoc />
        public override void Do()
        {
            base.Do();

            GlobalEditorHandler.CurrentCourseModified();
        }

        /// <inheritdoc />
        public override void Undo()
        {
            base.Undo();

            GlobalEditorHandler.CurrentCourseModified();
        }

        public CourseCommand(Action doCallback, Action undoCallback) : base(doCallback, undoCallback) { }
    }
}
