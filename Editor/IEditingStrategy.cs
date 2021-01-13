using Innoactive.Creator.Core;
using Innoactive.CreatorEditor.UI.Windows;

namespace Innoactive.CreatorEditor
{
    /// <summary>
    /// An interface for a strategy that defines how various events should be handled by the Creator editor.
    /// </summary>
    internal interface IEditingStrategy
    {
        /// <summary>
        /// Returns the current course.
        /// </summary>
        ICourse CurrentCourse { get; }

        /// <summary>
        /// Invoked when a new <see cref="CourseWindow"/> was just opened.
        /// </summary>
        void HandleNewCourseWindow(CourseWindow window);

        /// <summary>
        /// Invoked when a new <see cref="StepWindow"/> was just opened.
        /// </summary>
        void HandleNewStepWindow(StepWindow window);

        /// <summary>
        /// Invoked when a designer has just modified the course in the editor.
        /// </summary>
        void HandleCurrentCourseModified();

        /// <summary>
        /// Invoked when a <see cref="CourseWindow"/> was closed.
        /// </summary>
        void HandleCourseWindowClosed(CourseWindow window);

        /// <summary>
        /// Invoked when a <see cref="StepWindow"/> was closed.
        /// </summary>
        void HandleStepWindowClosed(StepWindow window);

        /// <summary>
        /// Invoked when user wants to start working on the current course.
        /// </summary>
        void HandleStartEditingCourse();

        /// <summary>
        /// Invoked when the currently edited course was changed to a different one.
        /// </summary>
        void HandleCurrentCourseChanged(string courseName);

        /// <summary>
        /// Invoked when the currently edited <see cref="IStep"/> was modified.
        /// </summary>
        void HandleCurrentStepModified(IStep step);

        /// <summary>
        /// Invoked when a designer wants to start working on a step.
        /// </summary>
        void HandleStartEditingStep();

        /// <summary>
        /// Invoked when a designer chooses a <see cref="IStep"/> to edit.
        /// </summary>
        void HandleCurrentStepChanged(IStep step);

        /// <summary>
        ///  Invoked when the project is going to be unloaded (when assemblies are unloaded, when user starts or stop runtime, when scripts were modified).
        /// </summary>
        void HandleProjectIsGoingToUnload();

        /// <summary>
        /// Invoked just before Unity saves the project (either during the normal exit of the Editor application or when the designer clicks `Save Project`).
        /// </summary>
        void HandleProjectIsGoingToSave();

        /// <summary>
        /// Invoked when exiting play mode.
        /// </summary>
        void HandleExitingPlayMode();

        /// <summary>
        /// Invoked when entering play mode.
        /// </summary>
        void HandleEnterPlayMode();
    }
}
