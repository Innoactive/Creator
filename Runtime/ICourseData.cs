using System.Collections.Generic;
using VPG.Creator.Core.EntityOwners;

namespace VPG.Creator.Core
{
    /// <summary>
    /// The data class for a <see cref="ICourse"/>.
    /// </summary>
    public interface ICourseData : IEntitySequenceDataWithMode<IChapter>, INamedData
    {
        /// <summary>
        /// The list of the <see cref="IChapter"/>s.
        /// </summary>
        IList<IChapter> Chapters { get; set; }

        /// <summary>
        /// The <see cref="IChapter"/> to start execution from.
        /// </summary>
        IChapter FirstChapter { get; }
    }
}
