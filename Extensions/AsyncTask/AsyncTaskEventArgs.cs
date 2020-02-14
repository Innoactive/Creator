using System;

namespace Innoactive.Hub.SDK
{
    /// <summary>
    /// Event arguments for IAsyncTask's <see cref="IAsyncTask.Aborted" />.
    /// </summary>
    public class AsyncTaskAbortedEventArgs : EventArgs
    {
    }

    /// <summary>
    /// Event arguments for IAsyncTask's <see cref="IAsyncTask.Started" />.
    /// </summary>
    public class AsyncTaskStartedEventArgs : EventArgs
    {
    }

    /// <summary>
    /// Event arguments for IAsyncTask's <see cref="IAsyncTask.Finished" />.
    /// </summary>
    public class AsyncTaskFinishedEventArgs : EventArgs
    {
    }

    /// <summary>
    /// Event arguments for IAsyncTask's <see cref="IAsyncTask.Errored" />.
    /// </summary>
    public class AsyncTaskErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Exception that caused the task to fail.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Constructor that sets <paramref name="exception"/> as value for <see cref="Exception"/>.
        /// </summary>
        public AsyncTaskErrorEventArgs(Exception exception)
        {
            Exception = exception;
        }
    }

    /// <summary>
    /// Event arguments for IAsyncTask's <see cref="IAsyncTask.ProgressUpdate" />.
    /// </summary>
    public class AsyncTaskProgressEventArgs : EventArgs
    {
        /// <summary>
        /// Current progress of the reporting task.
        /// </summary>
        public float Progress { get; }

        /// <summary>
        /// Constructor that sets <paramref name="progress"/> as value for <see cref="Progress"/>.
        /// </summary>
        public AsyncTaskProgressEventArgs(float progress)
        {
            Progress = progress;
        }
    }

    /// <summary>
    /// Event arguments for IAsyncTask's <see cref="IAsyncTask{T}.FinishedWithResult" />.
    /// </summary>
    public class AsyncTaskFinishedResultEventArgs<T> : AsyncTaskFinishedEventArgs
    {
        /// <summary>
        /// Result of the finished task.
        /// </summary>
        public T Result { get; }

        /// <summary>
        /// Constructor that sets <paramref name="result"/> as value for <see cref="Result"/>.
        /// </summary>
        public AsyncTaskFinishedResultEventArgs(T result)
        {
            Result = result;
        }
    }
}