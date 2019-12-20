using System.Collections.Generic;
using System.Linq;
using Innoactive.Hub.Training.Conditions;
using Innoactive.Hub.Training.Configuration.Modes;
using Innoactive.Hub.Training.EntityOwners;

namespace Innoactive.Hub.Training
{
    public interface ITransitionData : IEntityCollectionData<ICondition>, IModeData, ICompletableData
    {
        IList<ICondition> Conditions { get; set; }
        IStep TargetStep { get; set; }
    }

    /// <summary>
    /// An interface for a transition that determines when a <see cref="IStep"/> is completed and what is the next <see cref="IStep"/>.
    /// </summary>
    public interface ITransition : IEntity, ICompletable, IDataOwner<ITransitionData>
    {
    }
}
