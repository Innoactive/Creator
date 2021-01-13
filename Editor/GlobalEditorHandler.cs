using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using Innoactive.Creator.Core;
using Innoactive.CreatorEditor.UI.Windows;
using Innoactive.Creator.Core.Configuration;

namespace Innoactive.CreatorEditor
{
    /// <summary>
    /// A class that handles interactions between Creator windows and course assets by using selected <seealso cref="IEditingStrategy"/> strategy.
    /// </summary>
    [InitializeOnLoad]
    internal static class GlobalEditorHandler
    {
        internal const string LastEditedCourseNameKey = "Innoactive.Creator.Editors.LastEditedCourseName";

        private static IEditingStrategy strategy;

        static GlobalEditorHandler()
        {
            SetDefaultStrategy();

            string lastEditedCourseName = EditorPrefs.GetString(LastEditedCourseNameKey);
            SetCurrentCourse(lastEditedCourseName);

            EditorSceneManager.sceneOpened += OnSceneOpened;
        }

        /// <summary>
        /// Sets <see cref="DefaultEditingStrategy"/> as current strategy.
        /// </summary>
        internal static void SetDefaultStrategy()
        {
            SetStrategy(new DefaultEditingStrategy());
        }

        /// <summary>
        /// Sets given <see cref="IEditingStrategy"/> as current strategy.
        /// </summary>
        internal static void SetStrategy(IEditingStrategy newStrategy)
        {
            strategy = newStrategy;

            if (newStrategy == null)
            {
                Debug.LogError("An editing strategy cannot be null, set to default instead.");
                SetDefaultStrategy();
            }
        }

        /// <summary>
        /// Returns the current active course, can be null.
        /// </summary>
        internal static ICourse GetCurrentCourse()
        {
            return strategy.CurrentCourse;
        }

        /// <summary>
        /// Notifies selected <see cref="IEditingStrategy"/> when a new <see cref="CourseWindow"/> was just opened.
        /// </summary>
        internal static void CourseWindowOpened(CourseWindow window)
        {
            strategy.HandleNewCourseWindow(window);
        }

        /// <summary>
        /// Notifies selected <see cref="IEditingStrategy"/> when a <see cref="CourseWindow"/> was closed.
        /// </summary>
        internal static void CourseWindowClosed(CourseWindow window)
        {
            strategy.HandleCourseWindowClosed(window);
        }

        /// <summary>
        /// Notifies selected <see cref="IEditingStrategy"/> when a new <see cref="StepWindow"/> was just opened.
        /// </summary>
        internal static void StepWindowOpened(StepWindow window)
        {
            strategy.HandleNewStepWindow(window);
        }

        /// <summary>
        /// Notifies selected <see cref="IEditingStrategy"/> when a <see cref="StepWindow"/> was closed.
        /// </summary>
        internal static void StepWindowClosed(StepWindow window)
        {
            strategy.HandleStepWindowClosed(window);
        }

        /// <summary>
        /// Notifies selected <see cref="IEditingStrategy"/> when the currently edited course was changed to a different one.
        /// </summary>
        internal static void SetCurrentCourse(string courseName)
        {
            strategy.HandleCurrentCourseChanged(courseName);
        }

        /// <summary>
        /// Notifies selected <see cref="IEditingStrategy"/> when user wants to start working on the current course.
        /// </summary>
        internal static void StartEditingCourse()
        {
            strategy.HandleStartEditingCourse();
        }

        /// <summary>
        /// Notifies selected <see cref="IEditingStrategy"/> when a designer has just modified the course in the editor.
        /// </summary>
        internal static void CurrentCourseModified()
        {
            strategy.HandleCurrentCourseModified();
        }

        /// <summary>
        /// Notifies selected <see cref="IEditingStrategy"/> when the currently edited <see cref="IStep"/> was modified.
        /// </summary>
        internal static void CurrentStepModified(IStep step)
        {
            strategy.HandleCurrentStepModified(step);
        }

        /// <summary>
        /// Notifies selected <see cref="IEditingStrategy"/> when a designer chooses a <see cref="IStep"/> to edit.
        /// </summary>
        internal static void ChangeCurrentStep(IStep step)
        {
            strategy.HandleCurrentStepChanged(step);
        }

        /// <summary>
        /// Notifies selected <see cref="IEditingStrategy"/> when a designer wants to start working on a step.
        /// </summary>
        internal static void StartEditingStep()
        {
            strategy.HandleStartEditingStep();
        }

        /// <summary>
        /// Notifies selected <see cref="IEditingStrategy"/> when the project is going to be unloaded (when assemblies are unloaded, when user starts or stop runtime, when scripts were modified).
        /// </summary>
        internal static void ProjectIsGoingToUnload()
        {
            strategy.HandleProjectIsGoingToUnload();
        }

        /// <summary>
        /// Notifies selected <see cref="IEditingStrategy"/> before Unity saves the project (either during the normal exit of the Editor application or when the designer clicks `Save Project`).
        /// </summary>
        internal static void ProjectIsGoingToSave()
        {
            strategy.HandleProjectIsGoingToSave();
        }

        internal static void EnterPlayMode()
        {
            strategy.HandleEnterPlayMode();
        }

        internal static void ExitPlayMode()
        {
            strategy.HandleExitingPlayMode();
        }

        private static void OnSceneOpened(Scene scene, OpenSceneMode mode)
        {
            if (RuntimeConfigurator.Exists == false)
            {
                SetCurrentCourse(string.Empty);
                return;
            }

            string coursePath = RuntimeConfigurator.Instance.GetSelectedCourse();

            if (string.IsNullOrEmpty(coursePath))
            {
                SetCurrentCourse(string.Empty);
                return;
            }

            string courseName = System.IO.Path.GetFileNameWithoutExtension(coursePath);
            SetCurrentCourse(courseName);
        }
    }
}
