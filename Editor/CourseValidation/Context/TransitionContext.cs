using Innoactive.Creator.Core;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Base context for objects of type <see cref="ITransition"/>.
    /// </summary>
    public class TransitionContext : EntityDataContext<ITransitionData>
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
            foreach (ITransition transition in ((StepContext) Parent).EntityData.Transitions.Data.Transitions)
            {
                if (transition.Data == EntityData)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }
    }
}
