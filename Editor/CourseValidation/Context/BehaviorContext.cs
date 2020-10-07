using Innoactive.Creator.Core.Behaviors;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Base context for objects of type <see cref="IBehaviorData"/>.
    /// </summary>
    public class BehaviorContext : EntityContext<IBehaviorData>
    {
        /// <inheritdoc/>
        public override bool IsSelectable { get; } = false;

        public BehaviorContext(IBehaviorData behavior, StepContext parent) : base(behavior, parent) { }

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
                return Entity.ToString();
            }
            return Entity.Name;
        }
    }
}
