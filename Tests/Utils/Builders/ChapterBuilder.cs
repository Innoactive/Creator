using VPG.Core;

namespace VPG.Tests.Builder
{
    public abstract class ChapterBuilder<TChapter> : BuilderWithResourcePath<TChapter> where TChapter : IChapter
    {
        public ChapterBuilder(string resourceSubPath) : base(resourceSubPath)
        {

        }
    }
}
