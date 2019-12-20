using System.Collections.Generic;
using Innoactive.Hub.Training.Configuration.Modes;
using Innoactive.Hub.Training.EntityOwners;

namespace Innoactive.Hub.Training
{
    public interface ICourseData : IEntitySequenceData<IChapter>, INamedData, IModeData
    {
        IList<IChapter> Chapters { get; set; }

        IChapter FirstChapter { get; }
    }
}
