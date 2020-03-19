using System.Collections.Generic;
using Innoactive.Creator.Core.Configuration.Modes;
using Innoactive.Creator.Core.EntityOwners;

namespace Innoactive.Creator.Core
{
    public interface ICourseData : IEntitySequenceData<IChapter>, INamedData, IModeData
    {
        IList<IChapter> Chapters { get; set; }

        IChapter FirstChapter { get; }
    }
}
