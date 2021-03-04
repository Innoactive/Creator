using System;

namespace Innoactive.Creator.Core
{
    public class CourseEventArgs : EventArgs
    {
        public readonly ICourse Course;
        public readonly IChapter Chapter;
        public readonly IStep CurrentStep;

        public CourseEventArgs(ICourse course)
        {
            Course = course;
            Chapter = course.Data.Current;
            if (Chapter != null)
            {
                CurrentStep = Chapter.Data.Current;
            }
        }
    }
}
