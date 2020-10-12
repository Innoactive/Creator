using System.Collections.Generic;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Behaviors;
using Innoactive.Creator.Core.Conditions;
using Innoactive.CreatorEditor.CourseValidation;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Implementation of <see cref="BaseValidator{T,TContext}"/> used for validate logic conflicts.
    /// </summary>
    public abstract class BaseStepValidator : BaseValidator<IStepData, StepContext>
    {
        /// <summary>
        /// Retrieves a list of <see cref="IBehavior"/> of requested type from given <paramref name="step"/>.
        /// </summary>
        /// <param name="step">The <see cref="IStep"/> where to retrieve the list of <see cref="IBehavior"/>.</param>
        /// <typeparam name="T">The desired type of <see cref="IBehavior"/> to retrieve.</typeparam>
        protected List<T> GetBehavior<T>(IStepData step) where T : IBehavior
        {
            List<T> result = new List<T>();
            foreach (IBehavior behavior in step.Behaviors.Data.Behaviors)
            {
                if (behavior is T)
                {
                    result.Add((T)behavior);
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieves a list of <see cref="ICondition"/> of requested type from given <paramref name="transition"/>.
        /// </summary>
        /// <param name="transition">The <see cref="ITransition"/> where to retrieve the list of <see cref="ICondition"/>.</param>
        /// <typeparam name="T">The desired type of <see cref="ICondition"/> to retrieve.</typeparam>
        protected List<T> GetCondition<T>(ITransitionData transition) where T : ICondition
        {
            List<T> result = new List<T>();
            foreach (ICondition condition in transition.Conditions)
            {
                if (condition is T)
                {
                    result.Add((T)condition);
                }
            }

            return result;
        }
    }
}
