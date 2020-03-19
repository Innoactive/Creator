using System.Collections.Generic;
using Innoactive.Creator.Core.Configuration.Modes;
using Innoactive.Creator.Core.EntityOwners;

namespace Innoactive.Creator.Core
{
    public interface ITransitionCollectionData : IEntityCollectionData<ITransition>, IModeData
    {
        IList<ITransition> Transitions { get; set; }
    }
}
