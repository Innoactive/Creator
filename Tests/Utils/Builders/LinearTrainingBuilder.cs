using System.Collections.Generic;
using VPG.Creator.Core;

namespace VPG.Creator.Tests.Builder
{
    /// <summary>
    /// Builder that creates linear trainings.
    /// </summary>
    public class LinearTrainingBuilder : TrainingBuilder<Course>
    {
        public List<IChapter> Chapters { get; set; }

        /// <summary>
        /// The builder will create a training with given name.
        /// </summary>
        /// <param name="name">Name of the training.</param>
        public LinearTrainingBuilder(string name, string rootResourceFolder = "") : base(name)
        {
            Chapters = new List<IChapter>();
            AddFirstPassAction(() => SetRelativeResourcePathAction(() => rootResourceFolder));
            AddSecondPassAction(() => Result = new Course(name, Chapters));
        }

        /// <summary>
        /// Add an Action to an execution queue that makes necessary operations over a chapter it gets from the chapterBuilder.
        /// </summary>
        public LinearTrainingBuilder AddChapter<TChapter>(ChapterBuilder<TChapter> chapterBuilder) where TChapter : IChapter
        {
            AddFirstPassAction(() =>
            {
                chapterBuilder.SetRelativeResourcePathAction(() => ResourcePath);
            });

            AddFirstPassAction(() =>
            {
                Chapters.Add(chapterBuilder.Build());
            });
            return this;
        }

        public new LinearTrainingBuilder SetResourcePath(string path)
        {
            base.SetResourcePath(path);
            return this;
        }

        /// <inheritdoc />
        protected override void Cleanup()
        {
            base.Cleanup();
            Chapters.Clear();
        }

    }
}
