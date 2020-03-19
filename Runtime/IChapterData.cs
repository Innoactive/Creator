using System.Collections.Generic;
using Innoactive.Creator.Core.Configuration.Modes;
using Innoactive.Creator.Core.EntityOwners;

namespace Innoactive.Creator.Core
{
    public interface IChapterData : IEntitySequenceData<IStep>, INamedData, IModeData
    {
        IStep FirstStep { get; set; }

        IList<IStep> Steps { get; set; }
    }
}
