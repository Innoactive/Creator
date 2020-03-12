using System.Collections.Generic;
using Innoactive.Hub.Training.Configuration.Modes;
using Innoactive.Hub.Training.EntityOwners;

namespace Innoactive.Hub.Training
{
    public interface ITransitionCollectionData : IEntityCollectionData<ITransition>, IModeData
    {
        IList<ITransition> Transitions { get; set; }
    }
}