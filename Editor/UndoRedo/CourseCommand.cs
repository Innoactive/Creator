using System;
using Innoactive.CreatorEditor.UI.Windows;

namespace Innoactive.CreatorEditor.UndoRedo
{
    /// <summary>
    /// A <see cref="CallbackCommand"/> which marks <see cref="CourseWindow"/> as dirty.
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
