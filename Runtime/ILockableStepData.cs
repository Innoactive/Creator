using System.Collections.Generic;
using System.Runtime.Serialization;
using Innoactive.Creator.Core.Behaviors;

namespace Innoactive.Creator.Core
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
