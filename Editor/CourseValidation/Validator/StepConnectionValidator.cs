using System.Linq;
using System.Collections.Generic;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Validation;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Goes through a chapter and checks if every step is connected to the chapter entrypoint.
    /// </summary>
    internal class StepConnectionValidator : BaseValidator<IChapter, ChapterContext>
    {
        /// <inheritdoc/>
        protected override List<ValidationReportEntry> InternalValidate(IChapter chapter)
        {
            HashSet<IStep> connectedSteps = FindAllConnectedSteps(chapter);
            return ReportMissedSteps(chapter, connectedSteps);
        }

        private List<ValidationReportEntry> ReportMissedSteps(IChapter chapter, HashSet<IStep> connectedSteps)
        {
            List<ValidationReportEntry> result = new List<ValidationReportEntry>();

            foreach (IStep missedStep in chapter.Data.Steps.Except(connectedSteps))
            {
                result.Add(new ValidationReportEntry
                {
                    ErrorLevel = ValidationErrorLevel.WARNING,
                    Context = new StepContext(missedStep, Context),
                    Message = "Step is not reachable!",
                    Validator = this,
                });
            }

            return result;
        }

        private HashSet<IStep> FindAllConnectedSteps(IChapter chapter)
        {
            HashSet<IStep> connectedSteps = new HashSet<IStep>();
            connectedSteps.Add(chapter.Data.FirstStep);

            foreach (IStep step in chapter.Data.Steps)
            {
                foreach (ITransition transition in step.Data.Transitions.Data.Transitions)
                {
                    connectedSteps.Add(transition.Data.TargetStep);
                }
            }

            return connectedSteps;
        }
    }
}
