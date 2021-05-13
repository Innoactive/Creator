using System.Linq;
using UnityEditor;
using UnityEngine;
using VPG.Creator.Core;
using VPG.CreatorEditor.UI.Windows;
using VPG.CreatorEditor.Configuration;

namespace VPG.CreatorEditor
{
    /// <summary>
    /// This strategy is used by default and it handles interaction between course assets and various Creator windows.
    /// </summary>
    internal class DefaultEditingStrategy : IEditingStrategy
    {
        private CourseWindow courseWindow;
        private StepWindow stepWindow;

        public ICourse CurrentCourse { get; protected set; }
        public IChapter CurrentChapter { get; protected set; }

        /// <inheritdoc/>
        public void HandleNewCourseWindow(CourseWindow window)
        {
            courseWindow = window;
            courseWindow.SetCourse(CurrentCourse);
        }

        /// <inheritdoc/>
        public void HandleNewStepWindow(StepWindow window)
        {
            stepWindow = window;
            if (courseWindow == null || courseWindow.Equals(null))
            {
                HandleCurrentStepChanged(null);
            }
            else
            {
                HandleCurrentStepChanged(courseWindow.GetChapter().ChapterMetadata.LastSelectedStep);
            }
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

            if (CurrentCourse != null)
            {
                CourseAssetManager.Save(CurrentCourse);
            }
        }

        /// <inheritdoc/>
        public void HandleStepWindowClosed(StepWindow window)
        {
            if (CurrentCourse != null)
            {
                CourseAssetManager.Save(CurrentCourse);
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
            else
            {
                courseWindow.Focus();
            }
        }

        /// <inheritdoc/>
        public void HandleCurrentCourseChanged(string courseName)
        {
            if (CurrentCourse != null)
            {
                CourseAssetManager.Save(CurrentCourse);
            }

            EditorPrefs.SetString(GlobalEditorHandler.LastEditedCourseNameKey, courseName);
            LoadCourse(CourseAssetManager.Load(courseName));
        }

        private void LoadCourse(ICourse newCourse)
        {
            CurrentCourse = newCourse;
            CurrentChapter = null;

            if (newCourse != null && EditorConfigurator.Instance.Validation.IsAllowedToValidate())
            {
                EditorConfigurator.Instance.Validation.Validate(newCourse.Data, newCourse);
            }

            if (courseWindow != null)
            {
                courseWindow.SetCourse(CurrentCourse);
                if (stepWindow != null)
                {
                    stepWindow.SetStep(courseWindow.GetChapter()?.ChapterMetadata.LastSelectedStep);
                }
            }
            else if (stepWindow != null)
            {
                stepWindow.SetStep(null);
            }
        }

        /// <inheritdoc/>
        public void HandleCurrentStepModified(IStep step)
        {
            courseWindow.GetChapter().ChapterMetadata.LastSelectedStep = step;

            if (EditorConfigurator.Instance.Validation.IsAllowedToValidate())
            {
                EditorConfigurator.Instance.Validation.Validate(step.Data, CurrentCourse);
            }

            courseWindow.RefreshChapterRepresentation();
        }

        /// <inheritdoc/>
        public void HandleCurrentStepChanged(IStep step)
        {
            if (stepWindow != null)
            {
                if (step != null && EditorConfigurator.Instance.Validation.IsAllowedToValidate())
                {
                    EditorConfigurator.Instance.Validation.Validate(step.Data, CurrentCourse);
                }
                stepWindow.SetStep(step);
            }
        }

        /// <inheritdoc/>
        public void HandleStartEditingStep()
        {
            if (stepWindow == null)
            {
                StepWindow.ShowInspector();
            }
        }

        public void HandleCurrentChapterChanged(IChapter chapter)
        {
            CurrentChapter = chapter;
        }

        /// <inheritdoc/>
        public void HandleProjectIsGoingToUnload()
        {
            if (CurrentCourse != null)
            {
                CourseAssetManager.Save(CurrentCourse);
            }
        }

        /// <inheritdoc/>
        public void HandleProjectIsGoingToSave()
        {
            if (CurrentCourse != null)
            {
                CourseAssetManager.Save(CurrentCourse);
            }
        }

        /// <inheritdoc/>
        public void HandleExitingPlayMode()
        {
            if (stepWindow != null)
            {
                stepWindow.ResetStepView();
            }
        }

        /// <inheritdoc/>
        public void HandleEnterPlayMode()
        {
        }
    }
}
