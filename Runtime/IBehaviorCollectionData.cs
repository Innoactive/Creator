using System.Collections.Generic;
using Innoactive.Creator.Core.Behaviors;
using Innoactive.Creator.Core.EntityOwners;

namespace Innoactive.Creator.Core
{
    /// <summary>
    /// A data that contains a list of <see cref="IBehavior"/>s.
    /// </summary>
    public interface IBehaviorCollectionData : IEntityCollectionDataWithMode<IBehavior>
    {
        /// <summary>
        /// A list of <see cref="IBehavior"/>s.
        /// </summary>
        IList<IBehavior> Behaviors { get; set; }
    }
}
