using System.Collections.Generic;
using Innoactive.Creator.Core.EntityOwners;

namespace Innoactive.Creator.Core
{
    /// <summary>
    /// The interface of a data with a list of transitions.
    /// </summary>
    public interface ITransitionCollectionData : IEntityCollectionDataWithMode<ITransition>
    {
        /// <summary>
        /// A list of transitions.
        /// </summary>
        IList<ITransition> Transitions { get; set; }
    }
}
