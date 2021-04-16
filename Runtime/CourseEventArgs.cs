using System;

namespace Innoactive.Creator.Core
{
    /// <summary>
    /// EventArgs for course events.
    /// </summary>
    public class CourseEventArgs : EventArgs
    {
        /// <summary>
        /// Active course.
        /// </summary>
        public readonly ICourse Course;

        /// <summary>
        /// Active Chapter.
        /// </summary>
        public readonly IChapter Chapter;

        /// <summary>
        /// Active Step.
        /// </summary>
        public readonly IStep Step;

        public CourseEventArgs(ICourse course)
        {
            Course = course;
            Chapter = course.Data.Current;
            if (Chapter != null)
            {
                Step = Chapter.Data.Current;
            }
        }
    }
}
