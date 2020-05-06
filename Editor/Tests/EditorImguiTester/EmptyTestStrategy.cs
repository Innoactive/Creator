using Innoactive.Creator.Core;
using Innoactive.CreatorEditor.UI.Windows;

namespace Innoactive.CreatorEditor.TestTools
{
    public class EmptyTestStrategy : IEditingStrategy
    {
        public void HandleNewCourseWindow(CourseWindow window)
        {
        }

        public void HandleNewStepWindow(StepWindow window)
        {
        }

        public void HandleCurrentCourseModified()
        {
        }

        public void HandleCourseWindowClosed(CourseWindow window)
        {
        }

        public void HandleStepWindowClosed(StepWindow window)
        {
        }

        public void HandleStartEditingCourse()
        {
        }

        public void HandleCurrentCourseChanged(string courseName)
        {
        }

        public void HandleCurrentStepModified(IStep step)
        {
        }

        public void HandleStartEditingStep(IStep step)
        {
        }
    }
}
