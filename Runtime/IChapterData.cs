using System.Collections.Generic;
using VPG.Creator.Core.EntityOwners;

namespace VPG.Creator.Core
{
    /// <summary>
    /// The <see cref="IChapter"/>'s data interface.
    /// </summary>
    public interface IChapterData : IEntitySequenceDataWithMode<IStep>, INamedData
    {
        /// <summary>
        /// The <see cref="IStep"/> from which the chapter starts.
        /// </summary>
        IStep FirstStep { get; set; }

        /// <summary>
        /// The list of all <see cref="IStep"/>s in the chapter.
        /// </summary>
        IList<IStep> Steps { get; set; }
    }
}
