using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Utils;
using Innoactive.Creator.Core.Behaviors;
using Innoactive.Creator.Core.Conditions;
using Innoactive.Creator.Core.Validation;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Checks a Step data for empty variables.
    /// </summary>
    internal class MissingDataValidator : BaseValidator<IStepData, StepContext>
    {
        protected List<EditorReportEntry> result;

        /// <inheritdoc/>
        protected override List<EditorReportEntry> InternalValidate(IStepData step)
        {
            result = new List<EditorReportEntry>();

            foreach (IBehavior behavior in step.Behaviors.Data.Behaviors)
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
                IEnumerable<Attribute> attributes = memberInfo.GetCustomAttributes();

                if (attributes.Any(attribute => attribute is DataMemberAttribute))
                {
                    if (attributes.Any(attribute => attribute is OptionalValueAttribute))
                    {
                        continue;
                    }

                    ValidateRequiredValue(data, memberInfo, context);
                }
            }
        }

        private void ValidateRequiredValue(IData data, MemberInfo info, IContext context)
        {
            object value = ReflectionUtils.GetValueFromPropertyOrField(data, info);
            if (ReflectionUtils.IsEmpty(value))
            {
                ReportEntry entry = ReportEntryGenerator.VariableNotSet(info.Name);
                result.Add(new EditorReportEntry(new MemberInfoContext(info, data, context), this, entry));
            }

            if (ReflectionUtils.IsNumeric(value.GetType()) && value.Equals(ReflectionUtils.GetDefault(value.GetType())))
            {
                ReportEntry entry = ReportEntryGenerator.NumericVariableNotSet(info.Name);
                result.Add(new EditorReportEntry(new MemberInfoContext(info, data, context), this, entry));
            }
        }

    }
}
