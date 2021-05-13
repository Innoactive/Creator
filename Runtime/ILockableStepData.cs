using System.Collections.Generic;
using System.Runtime.Serialization;
using VPG.Creator.Core.Behaviors;

namespace VPG.Creator.Core
{
    /// <summary>
    /// Extends the step data with lockable data.
    /// </summary>
    internal interface ILockableStepData
    {
        /// <summary>
        /// Keeps all the lockable properties referenced which should be unlocked manually.
        /// </summary>
        [DataMember]
        IEnumerable<LockablePropertyReference> ToUnlock { get; set; }
    }
}
