using System.Collections.Generic;
using System.Linq;
using Innoactive.Creator.Core;

namespace Innoactive.CreatorEditor.CourseValidation
{
    public class ValidationReport
    {
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
