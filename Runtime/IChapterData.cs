using System.Collections.Generic;
using Innoactive.Creator.Core.EntityOwners;

namespace Innoactive.Creator.Core
{
    /// <summary>
    /// The chapter's data interface.
    /// </summary>
    public interface IChapterData : IEntitySequenceDataWithMode<IStep>, INamedData
    {
        /// <summary>
        /// The step from which the chapter starts.
        /// </summary>
        IStep FirstStep { get; set; }

        /// <summary>
        /// The list of all steps in the chapter.
        /// </summary>
        IList<IStep> Steps { get; set; }
    }
}
