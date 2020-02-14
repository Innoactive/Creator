namespace Innoactive.Hub.SDK
{
    /// <summary>
    /// Represents the different states in which an async task can exist.
    /// </summary>
    public enum AsyncTaskStatus
    {
        /// <summary>
        /// The task was created, but not started yet.
        /// </summary>
        Created = 0,
        /// <summary>
        /// The task was started and is still running.
        /// </summary>
        Running = 1,
        /// <summary>
        /// The task has finished successfully.
        /// </summary>
        Finished = 2,
        /// <summary>
        /// The task was aborted.
        /// </summary>
        Aborted = 3,
        /// <summary>
        /// The task has failed with an error.
        /// </summary>
        Errored = 4
    }
}