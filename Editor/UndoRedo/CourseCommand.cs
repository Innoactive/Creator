using System;
using Innoactive.CreatorEditor.UI.Windows;

namespace Innoactive.CreatorEditor.UndoRedo
{
    /// <summary>
    /// A <see cref="CallbackCommand"/> which notifies the <seealso cref="Editors"/> class that the current course was modified.
    /// </summary>
    public class CourseCommand : CallbackCommand
    {
        /// <inheritdoc />
        public override void Do()
        {
            base.Do();

            Editors.CurrentCourseModified();
        }

        /// <inheritdoc />
        public override void Undo()
        {
            base.Undo();

            Editors.CurrentCourseModified();
        }

        public CourseCommand(Action doCallback, Action undoCallback) : base(doCallback, undoCallback) { }
    }
}
