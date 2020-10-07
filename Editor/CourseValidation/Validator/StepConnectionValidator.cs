﻿using System.Linq;
using System.Collections.Generic;
using Innoactive.Creator.Core;
using Innoactive.Creator.Core.Validation;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Goes through a chapter and checks if every step is connected to the chapter entry point.
    /// </summary>
    internal class StepConnectionValidator : BaseValidator<IChapterData, ChapterContext>
    {
        /// <inheritdoc/>
        protected override List<EditorReportEntry> InternalValidate(IChapterData step)
        {
            HashSet<IStep> connectedSteps = FindAllConnectedSteps(step);
            return ReportMissedSteps(step, connectedSteps);
        }

        private List<EditorReportEntry> ReportMissedSteps(IChapterData chapter, HashSet<IStep> connectedSteps)
        {
            List<EditorReportEntry> result = new List<EditorReportEntry>();

            foreach (IStep missedStep in chapter.Steps.Except(connectedSteps))
            {
                result.Add(new EditorReportEntry
                {
                    ErrorLevel = ValidationErrorLevel.WARNING,
                    Code = 2001,
                    Context = new StepContext(missedStep.Data, Context),
                    Message = $"This Step: '{missedStep.Data.Name}' is not reachable!",
                    Validator = this,
                });
            }

            return result;
        }

        private HashSet<IStep> FindAllConnectedSteps(IChapterData chapter)
        {
            HashSet<IStep> connectedSteps = new HashSet<IStep>();
            connectedSteps.Add(chapter.FirstStep);

            foreach (IStep step in chapter.Steps)
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
