using Innoactive.Creator.Core;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Base context for objects of type <see cref="IChapterData"/>.
    /// </summary>
    public class ChapterContext : EntityContext<IChapterData>
    {
        public ChapterContext(IChapterData chapter, CourseContext parent) : base(chapter, parent) { }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (Parent != null)
            {
                return $"{Parent.ToString()} > [{Entity.Name}]";
            }
            return $"[{Entity.Name}]";
        }
    }
}
