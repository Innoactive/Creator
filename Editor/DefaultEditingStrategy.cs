using Innoactive.Creator.Core;
using Innoactive.CreatorEditor.UI.Windows;
using UnityEditor;

namespace Innoactive.CreatorEditor
{
    internal class DefaultEditingStrategy : IEditingStrategy
    {
        private CourseWindow courseWindow;
        private StepWindow stepWindow;
        private ICourse course;

        public void HandleNewCourseWindow(CourseWindow window)
        {
            if (courseWindow != null && courseWindow != window)
            {
                courseWindow.Close();
            }

            courseWindow = window;

            courseWindow.SetCourse(course);
        }

        public void HandleNewStepWindow(StepWindow window)
        {
            if (courseWindow == null)
            {
                window.Close();
                return;
            }

            if (stepWindow != null && stepWindow != window)
            {
                stepWindow.Close();
            }

            stepWindow = window;

            HandleStartEditingStep(courseWindow.GetChapter().ChapterMetadata.LastSelectedStep);
        }

        public void HandleCurrentCourseModified()
        {
            CourseAssetManager.Save(course);
        }

        public void HandleCourseWindowClosed(CourseWindow window)
        {
            if (stepWindow != null)
            {
                stepWindow.Close();
            }
        }

        public void HandleStepWindowClosed(StepWindow window)
        {
            if (stepWindow == window)
            {
                stepWindow = null;
            }
        }

        public void HandleStartEditingCourse()
        {
            CourseWindow.GetWindow();
        }

        public void HandleCurrentCourseChanged(string courseName)
        {
            EditorPrefs.SetString(Editors.LastEditedCourseNameKey, courseName);
            course = CourseAssetManager.Load(courseName);

            if (courseWindow != null)
            {
                courseWindow.SetCourse(course);
            }

            if (stepWindow != null)
            {
                stepWindow.SetStep(courseWindow.GetChapter()?.ChapterMetadata.LastSelectedStep);
            }
        }

        public void HandleCurrentStepModified(IStep step)
        {
            courseWindow.GetChapter().ChapterMetadata.LastSelectedStep = step;
            courseWindow.RefreshChapterRepresentation();
        }

        public void HandleStartEditingStep(IStep step)
        {
            if (stepWindow == null)
            {
                StepWindow.ShowInspector();
                return;
            }

            stepWindow.SetStep(step);
        }
    }
}
