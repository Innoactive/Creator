using VPG.Unity;

namespace VPG.Core
{
    /// <summary>
    /// Factory implementation for <see cref="IChapter"/> objects.
    /// </summary>
    internal class ChapterFactory : Singleton<ChapterFactory>
    {
        /// <summary>
        /// Creates a new <see cref="IChapter"/>.
        /// </summary>
        /// <param name="name"><see cref="IChapter"/>'s name.</param>
        public IChapter Create(string name)
        {
            return new Chapter(name, null);
        }
    }
}
