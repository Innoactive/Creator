using System;

namespace Innoactive.Hub.SDK
{
    /// <inheritdoc cref="AsyncTask{T}" />
    [Obsolete("Please directly use an AsyncTask<T> since the AsyncHttpTask<T> has become an empty wrapper class.")]
    public class AsyncHttpTask<T> : AsyncTask<T>, IAsyncHttpTask<T>
    {
        /// <summary>
        /// Creates a task that performs <paramref name="action"/>.
        /// </summary>
        public AsyncHttpTask(Func<IAsyncTask<T>, IDisposable> action) : base(action) { }

        /// <summary>
        /// Creates an empty task.
        /// </summary>
        public AsyncHttpTask() : base() { }

        /// <inheritdoc />
        public new IAsyncHttpTask<T> OnFinished(Action<T> onFinished)
        {
            return base.OnFinished(onFinished) as IAsyncHttpTask<T>;
        }

        /// <inheritdoc />
        public new IAsyncHttpTask<T> OnError(Action<Exception> onError)
        {
            return base.OnError(onError) as IAsyncHttpTask<T>;
        }

        /// <inheritdoc />
        public new IAsyncHttpTask<T> OnProgress(Action<float> onProgress)
        {
            return base.OnProgress(onProgress) as IAsyncHttpTask<T>;
        }

        /// <summary>
        /// Helper method that will return a task that only finished when all provided tasks have finished and
        /// will yield their results in the end
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static IAsyncHttpTask<T[]> WhenAll(params IAsyncHttpTask<T>[] tasks)
        {
            // wrap the resulting IAsyncTask so it is an IAsyncHttpTask again
            IAsyncHttpTask<T[]> castToAsyncHttpTask = new AsyncHttpTask<T[]>();
            castToAsyncHttpTask.TaskDelegate = () => AsyncTask<T>.WhenAll(tasks)
                .OnFinished(castToAsyncHttpTask.InvokeOnFinished)
                .OnError(castToAsyncHttpTask.InvokeOnError)
                .OnProgress(castToAsyncHttpTask.InvokeOnProgress)
                .Execute();
            return castToAsyncHttpTask;
        }

        /// <summary>
        /// Converts <paramref name="originalTask"/> into a <see cref="IAsyncHttpTask{T}"/>.
        /// </summary>
        public static IAsyncHttpTask<TT> FromIAsyncTask<TT>(IAsyncTask<TT> originalTask)
        {
            return new AsyncHttpTask<TT>(task => originalTask
                .OnFinished(task.InvokeOnFinished)
                .OnError(task.InvokeOnError)
                .OnProgress(task.InvokeOnProgress)
                .Execute());
        }
    }
}
