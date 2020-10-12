using Innoactive.Creator.Core;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Base context for objects of type <see cref="ITransition"/>.
    /// </summary>
    public class TransitionContext : EntityContext<ITransitionData>
    {
        public TransitionContext(ITransitionData transition, StepContext parent) : base(transition, parent) { }

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
            int index = 0;
            foreach (ITransition transition in ((StepContext) Parent).Entity.Transitions.Data.Transitions)
            {
                if (transition.Data == Entity)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }
    }
}
