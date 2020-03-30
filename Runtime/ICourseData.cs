using System.Collections.Generic;
using Innoactive.Creator.Core.EntityOwners;

namespace Innoactive.Creator.Core

{
    /// <summary>
    /// The data class for a course.
    /// </summary>
    public interface ICourseData : IEntitySequenceDataWithMode<IChapter>, INamedData
    {
        /// <summary>
        /// The list of the chapters.
        /// </summary>
        IList<IChapter> Chapters { get; set; }

        /// <summary>
        /// The chapter to start execution from.
        /// </summary>
        IChapter FirstChapter { get; }
    }
}
