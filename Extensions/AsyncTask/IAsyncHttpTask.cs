using System;

#pragma warning disable 1591

namespace Innoactive.Hub.SDK
{
    [Obsolete("Please directly use an IAsyncTask<T> since IAsyncHttpTask<T> has just become empty wrapper interface")]
    public interface IAsyncHttpTask<T> : IAsyncTask<T>
    {
        /// <inheritdoc cref="IAsyncTask{T}.OnFinished(Action{T})"/>
        new IAsyncHttpTask<T> OnFinished(Action<T> onFinished);

        /// <inheritdoc cref="IAsyncTask{T}.OnError"/>
        new IAsyncHttpTask<T> OnError(Action<Exception> onError);

        /// <inheritdoc cref="IAsyncTask{T}.OnProgress"/>
        new IAsyncHttpTask<T> OnProgress(Action<float> onProgress);
    }
}
