using Innoactive.Creator.Core.Conditions;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Base context for objects of type <see cref="ICondition"/>.
    /// </summary>
    public class ConditionContext : EntityContext<ICondition>
    {
        /// <inheritdoc/>
        public override bool IsSelectable { get; } = false;

        public ConditionContext(ICondition condition, TransitionContext parent) : base(condition, parent) { }

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
                return Entity.GetType().Name;
            }
            return Entity.Data.Name;
        }
    }
}
