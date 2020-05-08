using Innoactive.Creator.Core;
using Innoactive.CreatorEditor.UI.Windows;
using UnityEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor
{
    [InitializeOnLoad]
    internal static class GlobalEditorHandler
    {
        internal const string LastEditedCourseNameKey = "Innoactive.Creator.Editors.LastEditedCourseName";

        private static IEditingStrategy strategy;

        static GlobalEditorHandler()
        {
            SetDefaultStrategy();

            string lastEditedCourseName = PlayerPrefs.GetString(LastEditedCourseNameKey);
            SetCurrentCourse(lastEditedCourseName);
        }

        internal static void SetDefaultStrategy()
        {
            SetStrategy(new DefaultEditingStrategy());
        }

        internal static void SetStrategy(IEditingStrategy newStrategy)
        {
            strategy = newStrategy;

            if (newStrategy == null)
            {
                Debug.LogError("An editing strategy cannot be null, set to default instead.");
                SetDefaultStrategy();
            }
        }

        internal static void CourseWindowOpened(CourseWindow window)
        {
            strategy.HandleNewCourseWindow(window);
        }

        internal static void CourseWindowClosed(CourseWindow window)
        {
            strategy.HandleCourseWindowClosed(window);
        }

        internal static void StepWindowClosed(StepWindow window)
        {
            strategy.HandleStepWindowClosed(window);
        }

        internal static void StepWindowOpened(StepWindow window)
        {
            strategy.HandleNewStepWindow(window);
        }

        internal static void SetCurrentCourse(string courseName)
        {
            strategy.HandleCurrentCourseChanged(courseName);
        }

        internal static void StartEditingCourse()
        {
            strategy.HandleStartEditingCourse();
        }

        internal static void CurrentCourseModified()
        {
            strategy.HandleCurrentCourseModified();
        }

        internal static void CurrentStepModified(IStep step)
        {
            strategy.HandleCurrentStepModified(step);
        }

        internal static void StartEditingStep(IStep step)
        {
            strategy.HandleStartEditingStep(step);
        }

        internal static void ProjectIsGoingToUnload()
        {
            strategy.HandleProjectIsGoingToUnload();
        }

        internal static void ProjectIsGoingToSave()
        {
            strategy.HandleProjectIsGoingToSave();
        }
    }
}
