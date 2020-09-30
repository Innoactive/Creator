using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Behaviors;
using Innoactive.Creator.Core.Conditions;
using Innoactive.Creator.Core.Utils;
using Innoactive.Creator.Core.Validation;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Checks a Step data for attributes which implement <see cref="IAttributeValidator"/> and runs there valiation.
    /// </summary>
    internal class StepAttributeValidator : BaseValidator<IStep, StepContext>
    {
        protected List<ValidationReportEntry> result;


        /// <inheritdoc/>
        /// <inheritdoc/>
        protected override List<ValidationReportEntry> InternalValidate(IStep step)
        {
            result = new List<ValidationReportEntry>();
            foreach (IBehavior behavior in step.Data.Behaviors.Data.Behaviors)
            {
                Check(behavior.Data, new BehaviorContext(behavior, Context));
            }

            foreach (ITransition transition in step.Data.Transitions.Data.Transitions)
            {
                TransitionContext transitionContext = new TransitionContext(transition, Context);
                foreach (ICondition condition in  transition.Data.Conditions)
                {
                    Check(condition.Data, new ConditionContext(condition, transitionContext));
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
                    string message;
                    if (validator.Validate(value, out message))
                    {
                        result.Add(new ValidationReportEntry
                        {
                            ErrorLevel = validator.ErrorLevel,
                            Message = $"{message}",
                            Validator = this,
                            Context = new MemberInfoContext(memberInfo, context),
                        });
                    }
                }
            }
        }
    }
}
