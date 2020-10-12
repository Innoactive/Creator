using Innoactive.Creator.Core.Conditions;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Base context for objects of type <see cref="IConditionData"/>.
    /// </summary>
    public class ConditionContext : EntityDataContext<IConditionData>
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
            if (string.IsNullOrEmpty(EntityData.Name))
            {
                return EntityData.GetType().Name;
            }
            return EntityData.Name;
        }
    }
}
