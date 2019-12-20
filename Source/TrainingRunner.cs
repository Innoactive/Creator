using System.Collections.Generic;
using System.Linq;
using Innoactive.Hub.Training.Configuration;
using UnityEngine;

namespace Innoactive.Hub.Training
{
    public static class TrainingRunner
    {
        private class TrainingRunnerInstance : MonoBehaviour
        {
            public ICourse course = null;

            private void HandleModeChanged(object sender, ModeChangedEventArgs e)
            {
                if (course != null)
                {
                    course.Configure(e.Mode);
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

                if (course.LifeCycle.Stage == Stage.Active)
                {
                    course.LifeCycle.Deactivate();
                }
            }

            public void Execute()
            {
                RuntimeConfigurator.ModeChanged += HandleModeChanged;

                course.LifeCycle.StageChanged += HandleCourseStageChanged;
                course.Configure(RuntimeConfigurator.Configuration.GetCurrentMode());

                course.LifeCycle.Activate();
            }
        }

        private static TrainingRunnerInstance instance;

        public static bool IsRunning
        {
            get
            {
                return Current != null && Current.LifeCycle.Stage != Stage.Inactive;
            }
        }

        public static void Initialize(ICourse course)
        {
            instance = new GameObject("[TRAINING_RUNNER]").AddComponent<TrainingRunnerInstance>();

            instance.course = course;
        }

        public static void SkipChapters(int numberOfChapters)
        {
            IList<IChapter> chapters = Current.Data.Chapters;

            foreach (IChapter currentChapter in chapters.Skip(chapters.IndexOf(Current.Data.Current)).Take(numberOfChapters))
            {
                currentChapter.LifeCycle.MarkToFastForward();
            }
        }

        public static void SkipStep(ITransition transition)
        {
            if (IsRunning == false)
            {
                return;
            }

            Current.Data.Current.Data.Current.LifeCycle.MarkToFastForward();
            transition.Autocomplete();
        }

        public static void Run()
        {
            if (IsRunning)
            {
                return;
            }

            instance.Execute();
        }

        public static ICourse Current
        {
            get
            {
                return instance == null ? null : instance.course;
            }
        }
    }
}
