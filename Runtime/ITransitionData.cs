using System.Collections.Generic;
using Innoactive.Creator.Core.Conditions;
using Innoactive.Creator.Core.EntityOwners;

namespace Innoactive.Creator.Core
{
    // An interface for a transition's data.
    public interface ITransitionData : IEntityCollectionDataWithMode<ICondition>, ICompletableData
    {
        /// <summary>
        /// A list of conditions. When you complete all of them, this transition will trigger.
        /// </summary>
        IList<ICondition> Conditions { get; set; }

        /// <summary>
        /// The next step to take after this transition triggers.
        /// </summary>
        IStep TargetStep { get; set; }
    }
}
