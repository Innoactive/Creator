using Innoactive.Creator.Core;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Base context for objects of type <see cref="IStep"/>.
    /// </summary>
    public class StepContext : EntityContext<IStep>
    {
        /// <inheritdoc/>
        public override bool IsSelectable { get; } = false;

        public StepContext(IStep step, ChapterContext parent) : base(step, parent) { }

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
