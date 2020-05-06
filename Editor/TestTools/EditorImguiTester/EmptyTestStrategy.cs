using System.Runtime.CompilerServices;
using Innoactive.Creator.Core;
using Innoactive.CreatorEditor.UI.Windows;

[assembly: InternalsVisibleTo("Innoactive.Creator.Tests.Editmode")]

namespace Innoactive.CreatorEditor.TestTools
{
    internal class EmptyTestStrategy : IEditingStrategy
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

        public void HandleProjectIsGoingToUnload()
        {
        }

        public void HandleProjectIsGoingToSave()
        {
        }
    }
}
