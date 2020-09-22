using Innoactive.Creator.Core.Behaviors;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <inheritdoc/>
    public class BehaviorContext : EntityContext<IBehavior>
    {
        /// <inheritdoc/>
        public override bool IsSelectable { get; } = false;

        public BehaviorContext(IBehavior behavior, StepContext parent) : base(behavior, parent) { }

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
                return $"{Parent.ToString()} > [{GetName()}]";
            }
            return $"[{GetName()}]";
        }

        private string GetName()
        {
            if (string.IsNullOrEmpty(Entity.Data.Name))
            {
                return Entity.ToString();
            }
            return Entity.Data.Name;
        }
    }
}
