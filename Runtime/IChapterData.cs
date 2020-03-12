using System.Collections.Generic;
using Innoactive.Hub.Training.Configuration.Modes;
using Innoactive.Hub.Training.EntityOwners;

namespace Innoactive.Hub.Training
{
    public interface IChapterData : IEntitySequenceData<IStep>, INamedData, IModeData
    {
        IStep FirstStep { get; set; }

        IList<IStep> Steps { get; set; }
    }
}