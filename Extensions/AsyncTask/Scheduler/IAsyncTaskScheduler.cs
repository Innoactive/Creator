namespace Innoactive.Hub.SDK
{
    /// <summary>
    /// Base class for Schedulers for <see cref="AsyncTask"/>s that allow controlling
    /// the time and order in which tasks are executed.
    /// </summary>
    public interface IAsyncTaskScheduler
    {
        /// <summary>
        /// Register the AsyncTask for scheduling. The task may be executed immediately,
        /// or at a later time depending on the scheduling method.
        /// </summary>
        /// <typeparam name="T">return type of the task</typeparam>
        /// <returns>Task wrapping the passed task, that performs the scheduling on execution</returns>
        IAsyncTask<T> Schedule<T>(IAsyncTask<T> taskToSchedule);

        /// <summary>
        /// Register the AsyncTask for scheduling. The task may be executed immediately,
        /// or at a later time depending on the scheduling method.
        /// </summary>
        /// <returns>Task wrapping the passed task, that performs the scheduling on execution</returns>
        IAsyncTask Schedule(IAsyncTask taskToSchedule);
    }
}