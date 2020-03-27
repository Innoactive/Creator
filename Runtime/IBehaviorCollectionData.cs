using System.Collections.Generic;
using Innoactive.Creator.Core.Behaviors;
using Innoactive.Creator.Core.EntityOwners;

namespace Innoactive.Creator.Core
{
    /// <summary>
    /// A data that contains list of behaviors.
    /// </summary>
    public interface IBehaviorCollectionData : IEntityCollectionDataWithMode<IBehavior>
    {
        /// <summary>
        /// A list of behaviors.
        /// </summary>
        IList<IBehavior> Behaviors { get; set; }
    }
}
