using Innoactive.Creator.Core;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <inheritdoc/>
    public class TransitionContext : EntityContext<ITransition>
    {
        public override bool IsSelectable { get; } = false;

        public TransitionContext(ITransition transition, StepContext parent) : base(transition, parent) { }

        public override void Select()
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            if (Parent != null)
            {
                return $"{Parent.ToString()} > [Transition:{FindTransitionPosition()}]";
            }
            return $"[Transition]";
        }

        private int FindTransitionPosition()
        {
            return ((StepContext) Parent).Entity.Data.Transitions.Data.Transitions.IndexOf(Entity);
        }
    }
}
