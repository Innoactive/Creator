using System;
using System.Collections.Generic;
using System.Linq;
using Innoactive.Creator.Core.Configuration;
using Innoactive.Creator.Core.Configuration.Modes;
using UnityEngine;

namespace Innoactive.Creator.Core
{
    /// <summary>
    /// Runs a <see cref="ICourse"/>, expects to be run only once.
    /// </summary>
    public static class CourseRunner
    {
        private class CourseRunnerInstance : MonoBehaviour
        {
            /// <summary>
            /// Reference to the currently used <see cref="ICourse"/>.
            /// </summary>
            public ICourse course = null;

            private void HandleModeChanged(object sender, ModeChangedEventArgs args)
            {
                if (course != null)
                {
                    course.Configure(args.Mode);
                    RuntimeConfigurator.Configuration.StepLockHandling.Configure(RuntimeConfigurator.Configuration.Modes.CurrentMode);
                }
            }

            private void HandleCourseStageChanged(object sender, ActivationStateChangedEventArgs e)
            {
                if (e.Stage == Stage.Inactive)
                {
                    RuntimeConfigurator.ModeChanged -= HandleModeChanged;
                    Destroy(gameObject);
                }
            }

            private void Update()
            {
                if (course == null)
                {
                    return;
                }

                if (course.LifeCycle.Stage == Stage.Inactive)
                {
                    return;
                }

                course.Update();

                if (course.Data.Current?.LifeCycle.Stage == Stage.Activating)
                {
                    ChapterStarted?.Invoke(this, new CourseEventArgs(course));
                }

                if (course.Data.Current?.Data.Current?.LifeCycle.Stage == Stage.Activating)
                {
                    StepStarted?.Invoke(this, new CourseEventArgs(course));
                }

                if (course.LifeCycle.Stage == Stage.Active)
                {
                    course.LifeCycle.Deactivate();
                    RuntimeConfigurator.Configuration.StepLockHandling.OnCourseFinished(course);
                    CourseFinished?.Invoke(this, new CourseEventArgs(course));
                }
            }

            /// <summary>
            /// Starts the <see cref="ICourse"/>.
            /// </summary>
            public void Execute()
            {
                CourseSetup?.Invoke(this, new CourseEventArgs(course));

                RuntimeConfigurator.ModeChanged += HandleModeChanged;

                course.LifeCycle.StageChanged += HandleCourseStageChanged;
                course.Configure(RuntimeConfigurator.Configuration.Modes.CurrentMode);

                RuntimeConfigurator.Configuration.StepLockHandling.Configure(RuntimeConfigurator.Configuration.Modes.CurrentMode);
                RuntimeConfigurator.Configuration.StepLockHandling.OnCourseStarted(course);
                course.LifeCycle.Activate();

                CourseStarted?.Invoke(this, new CourseEventArgs(course));
            }
        }

        private static CourseRunnerInstance instance;

        /// <summary>
        /// Will be called before the course is setup internally.
        /// </summary>
        public static EventHandler<CourseEventArgs> CourseSetup;

        /// <summary>
        /// Will be called on course start.
        /// </summary>
        public static EventHandler<CourseEventArgs> CourseStarted;

        /// <summary>
        /// Will be called each time a chapter activates
        /// </summary>
        public static EventHandler<CourseEventArgs> ChapterStarted;

        /// <summary>
        /// Will be called each time a step activates
        /// </summary>
        public static EventHandler<CourseEventArgs> StepStarted;

        /// <summary>
        /// Will be called when the course finishes.
        /// </summary>
        public static EventHandler<CourseEventArgs> CourseFinished;

        /// <summary>
        /// Currently running <see cref="ICourse"/>
        /// </summary>
        public static ICourse Current
        {
            get
            {
                return instance == null ? null : instance.course;
            }
        }

        /// <summary>
        /// Returns true if the current <see cref="ICourse"/> is running.
        /// </summary>
        public static bool IsRunning
        {
            get
            {
                return Current != null && Current.LifeCycle.Stage != Stage.Inactive;
            }
        }

        /// <summary>
        /// Initializes the training runner by creating all required component in scene.
        /// </summary>
        /// <param name="course">The course which should be run.</param>
        public static void Initialize(ICourse course)
        {
            instance = instance == null ? new GameObject("[TRAINING_RUNNER]").AddComponent<CourseRunnerInstance>() : instance;
            instance.course = course;
        }

        /// <summary>
        /// Skips the given amount of chapters.
        /// </summary>
        /// <param name="numberOfChapters">Number of chapters.</param>
        public static void SkipChapters(int numberOfChapters)
        {
            IList<IChapter> chapters = Current.Data.Chapters;

            foreach (IChapter currentChapter in chapters.Skip(chapters.IndexOf(Current.Data.Current)).Take(numberOfChapters))
            {
                currentChapter.LifeCycle.MarkToFastForward();
            }
        }

        /// <summary>
        /// Skips the current step and uses given transition.
        /// </summary>
        /// <param name="transition">Transition which should be used.</param>
        public static void SkipStep(ITransition transition)
        {
            if (IsRunning == false)
            {
                return;
            }

            Current.Data.Current.Data.Current.LifeCycle.MarkToFastForward();
            transition.Autocomplete();
        }

        /// <summary>
        /// Starts the <see cref="ICourse"/>.
        /// </summary>
        public static void Run()
        {
            if (IsRunning)
            {
                return;
            }

            instance.Execute();
        }
    }
}
