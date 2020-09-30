using Innoactive.Creator.Core;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Base context for objects of type <see cref="ITransition"/>.
    /// </summary>
    public class TransitionContext : EntityContext<ITransition>
    {
        /// <inheritdoc />
        public override bool IsSelectable { get; } = false;

        public TransitionContext(ITransition transition, StepContext parent) : base(transition, parent) { }

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
