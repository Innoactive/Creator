namespace Innoactive.Creator.Core
{
    /// <summary>
    /// A chapter is a high-level grouping of several <see cref="IStep"/>s.
    /// </summary>
    public interface IChapter : IEntity, IDataOwner<IChapterData>
    {
        /// <summary>
        /// Utility data which is used by Training SDK custom editors.
        /// </summary>
        ChapterMetadata ChapterMetadata { get; }
    }
}
