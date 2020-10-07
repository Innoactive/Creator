using System.Collections.Generic;
using System.Linq;

namespace Innoactive.CreatorEditor.CourseValidation
{
    internal class ValidationUtils
    {
        public static string CreateStepTooltip(List<EditorReportEntry> entries, ChapterContext context)
        {
            if (entries.Count == 0)
            {
                return "";
            }

            List<EditorReportEntry> fittingEntries = entries.Where(entry => entry.Validator.ValidatedContext == context.GetType()).ToList();
            string tooltip = "";
            if (fittingEntries.Count > 0)
            {
                tooltip = CreateTooltip(fittingEntries);
            }

            List<EditorReportEntry> internalEntries = entries.Except(fittingEntries).ToList();
            if (internalEntries.Count > 0)
            {
                if (string.IsNullOrEmpty(tooltip) == false)
                {
                    tooltip += "\n\n";
                }

                if (internalEntries.Count == 1)
                {
                    tooltip += $"This Step has one internal error. Open the Step Inspector to fix it.";
                }
                else
                {
                    tooltip += $"This Step has {internalEntries.Count} errors. Open the Step Inspector to fix it.";
                }
            }

            return tooltip;
        }

        /// <summary>
        /// Creates a standard tooltip for a list of errors.
        /// </summary>
        public static string CreateTooltip(List<EditorReportEntry> entries)
        {
            if (entries.Count == 0)
            {
                return "";
            }

            entries.Sort((entry1, entry2) => entry1.ErrorLevel.CompareTo(entry2.ErrorLevel));

            string tooltip = "";
            entries.ForEach(entry =>
            {
                if (string.IsNullOrEmpty(tooltip) == false)
                {
                    tooltip += "\n\n";
                }
                tooltip += $"* {entry.Message}";
            });

            return tooltip;
        }
    }
}
