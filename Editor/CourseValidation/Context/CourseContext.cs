using Innoactive.Creator.Core;

namespace Innoactive.CreatorEditor.CourseValidation
{
    /// <summary>
    /// Base context for objects of type <see cref="ICourseData"/>.
    /// </summary>
    public class CourseContext : EntityDataContext<ICourseData>
    {
        public CourseContext(ICourseData course) : base(course, null) { }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"[{EntityData.Name}]";
        }
    }
}
