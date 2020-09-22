using Innoactive.Creator.Core;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <inheritdoc/>
    public class CourseContext : EntityContext<ICourse>
    {
        /// <inheritdoc/>
        public override bool IsSelectable { get; } = false;

        public CourseContext(ICourse course) : base(course, null) { }

        /// <inheritdoc/>
        public override void Select()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"[{Entity.Data.Name}]";
        }
    }
}
