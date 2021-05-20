using VPG.Core.Runtime.Utils;
using UnityEngine;

namespace VPG.Core.Utils.Logging
{
    /// <summary>
    /// ScriptableObject which allows you to configure what of the course life cycle should be logged.
    /// </summary>
    public class LifeCycleLoggingConfig : SettingsObject<LifeCycleLoggingConfig>
    {
        /// <summary>
        /// True, if behaviors are allowed to be logged.
        /// </summary>
        public bool LogBehaviors = false;

        /// <summary>
        /// True, if conditions are allowed to be logged.
        /// </summary>
        public bool LogConditions = false;

        /// <summary>
        /// True, if chapters are allowed to be logged.
        /// </summary>
        public bool LogChapters = true;

        /// <summary>
        /// True, if steps are allowed to be logged.
        /// </summary>
        public bool LogSteps = true;

        /// <summary>
        /// True, if transitions are allowed to be logged.
        /// </summary>
        public bool LogTransitions = false;
    }
}
