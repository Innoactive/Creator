using System.Collections.Generic;
using System.Linq;
using Innoactive.Creator.Core;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Report generated from validations of <see cref="IValidationScope"/> objects.
    /// </summary>
    /// <remarks>Contains a list of <see cref="EditorReportEntry"/>.</remarks>
    internal class ValidationReport
    {
        /// <summary>
        /// List of <see cref="EditorReportEntry"/> generated from the <see cref="IValidationScope"/>'s validation.
        /// </summary>
        public List<EditorReportEntry> Entries { get; }

        /// <summary>
        /// Time spent on generation of this report in milliseconds.
        /// </summary>
        public long GenerationTime { get; }

        public ValidationReport(List<EditorReportEntry> entries, long generationTime)
        {
            Entries = entries;
            GenerationTime = generationTime;
        }

        /// <summary>
        /// Returns all <see cref="EditorReportEntry"/> found for given step.
        /// </summary>
        public List<EditorReportEntry> GetEntriesFor(IStep step)
        {
            return Entries.Where(entry =>
            {
                if (entry.Context is StepContext context)
                {
                    return context.Entity == step;
                }

                return false;
            }).ToList();
        }
    }
}
