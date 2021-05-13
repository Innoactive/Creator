using VPG.Creator.Core;

namespace VPG.Creator.Tests.Builder
{
    public abstract class ChapterBuilder<TChapter> : BuilderWithResourcePath<TChapter> where TChapter : IChapter
    {
        public ChapterBuilder(string resourceSubPath) : base(resourceSubPath)
        {

        }
    }
}
