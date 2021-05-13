using System.Collections.Generic;
using VPG.Creator.Core.Behaviors;
using VPG.Creator.Core.EntityOwners;

namespace VPG.Creator.Core
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
