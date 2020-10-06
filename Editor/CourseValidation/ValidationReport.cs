using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public List<EditorReportEntry> Entries { get; protected set; }

        /// <summary>
        /// Time spent on generation of this report in milliseconds.
        /// </summary>
        public long GenerationTime { get; protected set; }

        public ValidationReport(List<EditorReportEntry> entries, long generationTime)
        {
            Entries = entries;
            GenerationTime = generationTime;
        }

        /// <summary>
        /// Returns all <see cref="EditorReportEntry"/> found for given step.
        /// </summary>
        public List<EditorReportEntry> GetEntriesFor(IContext context)
        {
            return Entries.Where(entry => entry.Context.Equals(context) || entry.Context.IsChildOf(context)).ToList();
        }

        /// <summary>
        /// Returns <see cref="EditorReportEntry"/> for given context and step.
        /// </summary>
        public List<EditorReportEntry> GetContextEntriesFor<T>(IStepData step) where T : IContext
        {
            List<EditorReportEntry> entries = GetEntriesFor(new StepContext(step, null));
            return entries.Where(entry => entry.Context.ContainsContext<T>()).ToList();
        }

        /// <summary>
        /// Returns all <see cref="EditorReportEntry"/> found for given step.
        /// </summary>
        public List<EditorReportEntry> GetEntriesFor(IData data, MemberInfo info)
        {
            return Entries.Where(entry =>
            {
                if (entry.Context is MemberInfoContext context)
                {
                    if (context.ParentData == data && info.Name.Equals(context.MemberInfo.Name))
                    {
                        return true;
                    }
                }

                return false;
            }).ToList();
        }

        public void Update(List<EditorReportEntry> entries, IContext context, long generationTime)
        {
            Entries = Entries.Where(entry => entry.Context.IsChildOf(context) == false).ToList();
            Entries.AddRange(entries);
            GenerationTime = generationTime;
        }
    }
}
