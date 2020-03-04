using System.Collections.Generic;
using Innoactive.Hub.Training.Behaviors;
using Innoactive.Hub.Training.Configuration.Modes;
using Innoactive.Hub.Training.EntityOwners;

namespace Innoactive.Hub.Training
{
    public interface IBehaviorCollectionData : IEntityCollectionData<IBehavior>, IModeData
    {
        IList<IBehavior> Behaviors { get; set; }
    }
}