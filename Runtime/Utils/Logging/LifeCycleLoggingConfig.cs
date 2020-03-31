using UnityEngine;

namespace Innoactive.Creator.Core.Utils.Logging
{
    /// <summary>
    /// ScriptableObject which allows you to configure what of the course life cycle should be logged.
    /// </summary>
    public class LifeCycleLoggingConfig : ScriptableObject
    {
        private static LifeCycleLoggingConfig instance;

        /// <summary>
        /// Single instance which is used to configure logging.
        /// </summary>
        public static LifeCycleLoggingConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<LifeCycleLoggingConfig>("LifeCycleLoggingConfig");
                    if (instance == null)
                    {
                        // Create an instance of the logging which does not log anything.
                        instance = CreateInstance<LifeCycleLoggingConfig>();
                        instance.LogConditions = false;
                        instance.LogChapters = false;
                    }
                }

                return instance;
            }
        }

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
