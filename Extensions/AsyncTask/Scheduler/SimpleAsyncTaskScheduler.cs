namespace Innoactive.Hub.SDK
{
    /// <summary>
    /// AsyncTaskScheduler that does not perform any actual scheduling, but immediately sends
    /// any scheduled HttpRequests.
    /// </summary>
    public class SimpleAsyncTaskScheduler : IAsyncTaskScheduler
    {
        /// <inheritdoc />
        public IAsyncTask<T> Schedule<T>(IAsyncTask<T> taskToSchedule)
        {
            return taskToSchedule;
        }

        /// <inheritdoc />
        public IAsyncTask Schedule(IAsyncTask taskToSchedule)
        {
            return taskToSchedule;
        }
    }
}