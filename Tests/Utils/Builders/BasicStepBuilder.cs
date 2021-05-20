using System.Linq;
using VPG.Core;
using VPG.Core.Behaviors;
using VPG.Core.Conditions;
using VPG.Core.Configuration;
using VPG.Core.SceneObjects;

namespace VPG.Tests.Builder
{
    /// <summary>
    /// Basic step builder that creates step of type <typeparamref name="Step" />.
    /// </summary>
    public class BasicStepBuilder : StepBuilder<Step>
    {
        #region private static methods
        private static ISceneObject GetFromRegistry(string name)
        {
            return RuntimeConfigurator.Configuration.SceneObjectRegistry[name];
        }
        #endregion

        /// <summary>
        /// This builder will create step with given name.
        /// </summary>
        /// <param name="name">Name of a step.</param>
        public BasicStepBuilder(string name) : base(name)
        {
            Cleanup();

            AddSecondPassAction(() => Result = new Step(name));
            AddSecondPassAction(() => Result.Data.Transitions.Data.Transitions.Add(new Transition()));
        }

        #region public methods
        /// <summary>
        /// Add behavior to a step.
        /// </summary>
        /// <param name="behavior">Behavior to add.</param>
        /// <returns>This instance to support method chaining pattern.</returns>
        public BasicStepBuilder AddBehavior(IBehavior behavior)
        {
            AddSecondPassAction(() => Result.Data.Behaviors.Data.Behaviors.Add(behavior));
            return this;
        }

        /// <summary>
        /// Add condition to a step.
        /// </summary>
        /// <param name="condition">Condition to add.</param>
        /// <returns>This instance to support method chaining pattern.</returns>
        public BasicStepBuilder AddCondition(ICondition condition)
        {
            AddSecondPassAction(() => Result.Data.Transitions.Data.Transitions.First().Data.Conditions.Add(condition));
            return this;
        }
        #endregion
    }
}
