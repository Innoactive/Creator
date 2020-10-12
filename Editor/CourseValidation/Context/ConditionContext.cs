using Innoactive.Creator.Core.Conditions;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Base context for objects of type <see cref="IConditionData"/>.
    /// </summary>
    public class ConditionContext : EntityContext<IConditionData>
    {
        public ConditionContext(IConditionData condition, TransitionContext parent) : base(condition, parent) { }

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
