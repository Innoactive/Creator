using System.Collections.Generic;
using Innoactive.Creator.Core.Behaviors;
using Innoactive.Creator.Core.Configuration.Modes;
using Innoactive.Creator.Core.EntityOwners;

namespace Innoactive.Creator.Core
{
    public interface IBehaviorCollectionData : IEntityCollectionData<IBehavior>, IModeData
    {
        IList<IBehavior> Behaviors { get; set; }
    }
}
