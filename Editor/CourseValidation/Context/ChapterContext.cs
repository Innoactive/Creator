using Innoactive.Creator.Core;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <inheritdoc/>
    public class ChapterContext : EntityContext<IChapter>
    {
        /// <inheritdoc/>
        public override bool IsSelectable { get; } = false;

        public ChapterContext(IChapter chapter, CourseContext parent) : base(chapter, parent) { }

        /// <inheritdoc/>
        public override void Select()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (Parent != null)
            {
                return $"{Parent.ToString()} > [{Entity.Data.Name}]";
            }
            return $"[{Entity.Data.Name}]";
        }
    }
}
