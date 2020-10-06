using Innoactive.Creator.Core.Conditions;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Base context for objects of type <see cref="ICondition"/>.
    /// </summary>
    public class ConditionContext : EntityContext<IConditionData>
    {
        /// <inheritdoc/>
        public override bool IsSelectable { get; } = false;

        public ConditionContext(IConditionData condition, TransitionContext parent) : base(condition, parent) { }

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
            if (string.IsNullOrEmpty(Entity.Name))
            {
                return Entity.GetType().Name;
            }
            return Entity.Name;
        }
    }
}
