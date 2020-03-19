using System.Collections.Generic;
using Innoactive.Creator.Core.Conditions;
using Innoactive.Creator.Core.Configuration.Modes;
using Innoactive.Creator.Core.EntityOwners;

namespace Innoactive.Creator.Core
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
