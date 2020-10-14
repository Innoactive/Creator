using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Utils;
using Innoactive.Creator.Core.Behaviors;
using Innoactive.Creator.Core.Conditions;
using Innoactive.Creator.Core.Validation;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Checks a Step data for attributes which implement <see cref="IAttributeValidator"/> and runs their validation.
    /// </summary>
    internal class StepAttributeValidator : BaseStepValidator
    {
        protected List<EditorReportEntry> result;

        /// <inheritdoc/>
        protected override List<EditorReportEntry> InternalValidate(IStepData step)
        {
            result = new List<EditorReportEntry>();
            foreach (IBehavior behavior in GetAllBehaviors(step))
            {
                Check(behavior.Data, new BehaviorContext(behavior.Data, Context));
            }

            foreach (ITransition transition in step.Transitions.Data.Transitions)
            {
                TransitionContext transitionContext = new TransitionContext(transition.Data, Context);
                foreach (ICondition condition in transition.Data.Conditions)
                {
                    Check(condition.Data, new ConditionContext(condition.Data, transitionContext));
                }
            }

            return result;
        }

        private void Check(IData data, IContext context)
        {
            IEnumerable<MemberInfo> members = EditorReflectionUtils.GetAllFieldsAndProperties(data);
            foreach (MemberInfo memberInfo in members)
            {
                IEnumerable<IAttributeValidator> validators = memberInfo.GetCustomAttributes()
                    .Where(attribute => attribute.GetType().GetInterfaces().Contains(typeof(IAttributeValidator)))
                    .Cast<IAttributeValidator>();

                foreach (IAttributeValidator validator in validators)
                {
                    object value = ReflectionUtils.GetValueFromPropertyOrField(data, memberInfo);
                    validator.Validate(value).ForEach(report =>
                    {
                        result.Add(new EditorReportEntry(Context, this, report));
                    });
                }
            }
        }
    }
}
