using System;

namespace VPG.Core
{
    /// <summary>
    /// EventArgs for fast forward course events.
    /// </summary>
    public class FastForwardCourseEventArgs : CourseEventArgs
    {
        /// <summary>
        /// Completed transition
        /// </summary>
        public readonly ITransition CompletedTransition;

        public FastForwardCourseEventArgs(ITransition transition, ICourse course) : base(course)
        {
            CompletedTransition = transition;
        }
    }
}
