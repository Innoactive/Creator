using Innoactive.Creator.Core;
using Innoactive.CreatorEditor.UI.Windows;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor
{
    /// <summary>
    /// This strategy is used by default and it handles interaction between course assets and various Creator windows.
    /// </summary>
    internal class DefaultEditingStrategy : IEditingStrategy
    {
        private CourseWindow courseWindow;
        private StepWindow stepWindow;
        private ICourse course;

        /// <inheritdoc/>
        public void HandleNewCourseWindow(CourseWindow window)
        {
            if (courseWindow != null && courseWindow != window)
            {
                courseWindow.Close();
            }

            courseWindow = window;

            courseWindow.SetCourse(course);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public void HandleCurrentCourseModified()
        {
        }

        /// <inheritdoc/>
        public void HandleCourseWindowClosed(CourseWindow window)
        {
            if (courseWindow != window)
            {
                return;
            }

            if (course != null)
            {
                CourseAssetManager.Save(course);
            }

            if (stepWindow != null)
            {
                stepWindow.Close();
            }
        }

        /// <inheritdoc/>
        public void HandleStepWindowClosed(StepWindow window)
        {
            if (stepWindow != window)
            {
                return;
            }

            if (course != null)
            {
                CourseAssetManager.Save(course);
            }

            stepWindow = null;
        }

        /// <inheritdoc/>
        public void HandleStartEditingCourse()
        {
            if (courseWindow == null)
            {
                courseWindow = EditorWindow.GetWindow<CourseWindow>();
                courseWindow.minSize = new Vector2(400f, 100f);
            }
        }

        /// <inheritdoc/>
        public void HandleCurrentCourseChanged(string courseName)
        {
            if (course != null)
            {
                CourseAssetManager.Save(course);
            }

            PlayerPrefs.SetString(GlobalEditorHandler.LastEditedCourseNameKey, courseName);
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

        /// <inheritdoc/>
        public void HandleCurrentStepModified(IStep step)
        {
            courseWindow.GetChapter().ChapterMetadata.LastSelectedStep = step;
            courseWindow.RefreshChapterRepresentation();
        }

        /// <inheritdoc/>
        public void HandleStartEditingStep(IStep step)
        {
            if (stepWindow == null)
            {
                StepWindow.ShowInspector();
                return;
            }

            stepWindow.SetStep(step);
        }

        /// <inheritdoc/>
        public void HandleProjectIsGoingToUnload()
        {
            if (course != null)
            {
                CourseAssetManager.Save(course);
            }
        }

        /// <inheritdoc/>
        public void HandleProjectIsGoingToSave()
        {
            if (course != null)
            {
                CourseAssetManager.Save(course);
            }
        }
    }
}
