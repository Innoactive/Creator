using Innoactive.Creator.Core;
using Innoactive.CreatorEditor.UI.Windows;

namespace Innoactive.CreatorEditor
{
    internal interface IEditingStrategy
    {
        void HandleNewCourseWindow(CourseWindow window);
        void HandleNewStepWindow(StepWindow window);
        void HandleCurrentCourseModified();
        void HandleCourseWindowClosed(CourseWindow window);
        void HandleStepWindowClosed(StepWindow window);
        void HandleStartEditingCourse();
        void HandleCurrentCourseChanged(string courseName);
        void HandleCurrentStepModified(IStep step);
        void HandleStartEditingStep(IStep step);
    }
}
