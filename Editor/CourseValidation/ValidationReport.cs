using System.Collections.Generic;
using System.Linq;
using Innoactive.Creator.Core;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Report generated from validations of <see cref="IValidationScope"/> objects.
    /// </summary>
    /// <remarks>Contains a list of <see cref="ValidationReportEntry"/>.</remarks>
    public class ValidationReport
    {
        /// <summary>
        /// List of <see cref="ValidationReportEntry"/> generated from the <see cref="IValidationScope"/>'s validation.
        /// </summary>
        public List<ValidationReportEntry> Entries { get; }

        /// <summary>
        /// Time spend on generation of this report in milliseconds.
        /// </summary>
        public long GenerationTime { get; }

        public ValidationReport(List<ValidationReportEntry> entries, long generationTime)
        {
            Entries = entries;
            GenerationTime = generationTime;
        }

        /// <summary>
        /// Returns all <see cref="ValidationReportEntry"/> found for given step.
        /// </summary>
        public List<ValidationReportEntry> GetEntriesFor(IStep step)
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
