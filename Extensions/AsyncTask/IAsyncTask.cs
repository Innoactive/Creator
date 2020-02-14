using System;

namespace Innoactive.Hub.SDK
{
    /// <summary>
    /// Represents an asynchronous tasks that will call any number of registered handlers as soon
    /// as it is finished and emit an according event. Use the Finished event or <see cref="OnFinished(System.Action{object})"/>
    /// to run code as soon as the task has finished. During the task's operation, the task emits
    /// progress updates to any listeners attached to the <see cref="ProgressUpdate"/> event and any handler registered via
    /// <see cref="OnProgress"/>. In case of an exception the Error event will be emitted and any callbacks
    /// attached via <see cref="OnError"/> will be executed.
    /// </summary>
    public interface IAsyncTask
    {
        /// <summary>
        /// Event emitted whenever the progress of the task changes
        /// </summary>
        event EventHandler<AsyncTaskProgressEventArgs> ProgressUpdate;

        /// <summary>
        /// Event emitted when this task has been started
        /// </summary>
        event EventHandler<AsyncTaskStartedEventArgs> Started;

        /// <summary>
        /// Event emitted when this task is completed
        /// </summary>
        event EventHandler<AsyncTaskFinishedEventArgs> Finished;

        /// <summary>
        /// Event emitted when this task is completed, contains the result of the task
        /// </summary>
        event EventHandler<AsyncTaskFinishedResultEventArgs<object>> FinishedWithResult;

        /// <summary>
        /// Event emitted when this task has errored
        /// </summary>
        event EventHandler<AsyncTaskErrorEventArgs> Errored;

        /// <summary>
        /// Event emitted when this task has errored
        /// </summary>
        event EventHandler<AsyncTaskAbortedEventArgs> Aborted;

        /// <summary>
        /// The result of the async task. Only available after the task has finished
        /// </summary>
        object Result { get; }

        /// <summary>
        /// The current progress state of this task (0...1)
        /// </summary>
        float Progress { get; }

        /// <summary>
        /// Whether or not the Task is already finished
        /// </summary>
        bool IsFinished { get; }

        /// <summary>
        /// Whether or not the Task is currently running (executed)
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Whether or not the Task has errored
        /// </summary>
        bool IsErrored { get; }

        /// <summary>
        /// Whether or not the Task has been aborted
        /// </summary>
        bool IsAborted { get; }

        /// <summary>
        /// Registers a new callback to be executed as soon as the task has finished.
        /// </summary>
        /// <param name="onFinished">The callback action to be run upon task completion.</param>
        /// <returns>The instance of the task to easily chain method calls.</returns>
        IAsyncTask OnFinished(Action onFinished);

        /// <summary>
        /// Marks the task as finished and executes all registered handlers for the <seealso cref="FinishedWithResult"/> event.
        /// </summary>
        /// <param name="onFinished">The callback action to be run upon task completion.</param>
        /// <returns>The instance of the task to easily chain method calls.</returns>
        IAsyncTask OnFinished(Action<object> onFinished);

        /// <summary>
        /// Registers a new callback to be executed when an error occurs while executing the task.
        /// </summary>
        /// <param name="onError">The callback action to be run upon an exception in the task.</param>
        /// <returns>The instance of the task to easily chain method calls.</returns>
        IAsyncTask OnError(Action<Exception> onError);

        /// <summary>
        /// Registers a new callback to be executed whenever the progress (0...1) of the task execution changes.
        /// Can be used e.g. to visualize the progress as a loading bar.
        /// </summary>
        /// <param name="onProgress">The callback action to be run upon progress updates.</param>
        /// <returns>The instance of the task to easily chain method calls.</returns>
        IAsyncTask OnProgress(Action<float> onProgress);

        /// <summary>
        /// Actually runs the delegate and returns its result (an IDisposable object)
        /// that is to be disposed only after the OnFinished or OnError callback has
        /// been executed.
        /// </summary>
        /// <returns>A Disposable that can be used to abort the running task (may be null).</returns>
        IDisposable Execute();

        /// <summary>
        /// This is the actual delegate that contains the "logic" of the task
        /// and will run in an asynchronous manner. The TaskDelegate needs to take
        /// care of calling the respective OnProgress, OnError and OnFinished
        /// handlers attached to this AsyncTask.
        /// </summary>
        [Obsolete("Setting the task's delegate via the property is no longer supported. Please use the Task's constructor instead!")]
        Func<IDisposable> TaskDelegate { set; }

        /// <summary>
        /// Aborts the asynchronously running task by disposing it's disposable delegate.
        /// </summary>
        void Abort();

        /// <summary>
        /// Marks the task as finished and executes all registered handlers for the <seealso cref="Finished"/> event.
        /// </summary>
        void InvokeOnFinished();

        /// <summary>
        /// Marks the task as finished with the given result and executes all registered handlers for the <seealso cref="FinishedWithResult"/> event.
        /// </summary>
        /// <param name="result">The result instance with which to complete the task.</param>
        void InvokeOnFinished(object result);

        /// <summary>
        /// Marks this task as errored and notifies all listeners (<seealso cref="Errored"/>) of the provided exception that happened during task execution.
        /// </summary>
        /// <param name="error">The exception that occured while running the task.</param>
        void InvokeOnError(Exception error);

        /// <summary>
        /// Updates the task's progress and notifies any listeners (<seealso cref="ProgressUpdate"/>) of changed progress on the async task.
        /// </summary>
        /// <param name="progress">The progress (0...1).</param>
        void InvokeOnProgress(float progress);
    }

    /// <inheritdoc />
    /// <summary>
    /// Generic extension of an <see cref="IAsyncTask"/> that returns a result from the executed task.
    /// </summary>
    /// <typeparam name="T">The type of result to return.</typeparam>
    public interface IAsyncTask<T> : IAsyncTask
    {
        /// <summary>
        /// Event emitted when this task is completed, contains the result of the task.
        /// </summary>
        new event EventHandler<AsyncTaskFinishedResultEventArgs<T>> FinishedWithResult;

        /// <summary>
        /// The result of the async task. Only available after the task has finished.
        /// </summary>
        new T Result { get; }

        /// <summary>
        /// Marks the task as finished and executes all registered handlers for the <seealso cref="FinishedWithResult"/> event.
        /// </summary>
        /// <param name="onFinished">The callback action to be run upon task completion.</param>
        /// <returns>The instance of the task to easily chain method calls.</returns>
        IAsyncTask<T> OnFinished(Action<T> onFinished);

        /// <summary>
        /// Registers a new callback to be executed when an error occurs while executing the task.
        /// </summary>
        /// <param name="onError">The callback action to be run upon an exception in the task.</param>
        /// <returns>The instance of the task to easily chain method calls.</returns>
        new IAsyncTask<T> OnError(Action<Exception> onError);

        /// <summary>
        /// Registers a new callback to be executed whenever the progress (0...1) of the task execution changes.
        /// Can be used e.g. to visualize the progress as a loading bar.
        /// </summary>
        /// <param name="onProgress">The callback action to be run upon progress updates.</param>
        /// <returns>The instance of the task to easily chain method calls.</returns>
        new IAsyncTask<T> OnProgress(Action<float> onProgress);

        /// <summary>
        /// Marks the task as finished with the given result and executes all registered handlers for the <seealso cref="FinishedWithResult"/> event.
        /// </summary>
        /// <param name="result">The result instance with which to complete the task.</param>
        void InvokeOnFinished(T result);

        /// <summary>
        /// Converts the result of this asynchronous task to the given <typeparamref name="TOther"/> using the provided <paramref name="taskResultConversionFunction"/>.
        /// </summary>
        /// <typeparam name="TOther">The desired type of result.</typeparam>
        /// <param name="taskResultConversionFunction">The function used to convert the result of type <typeparamref name="T"/> to <typeparamref name="TOther"/>.</param>
        /// <returns>An async task that will return a result of the desired type <typeparamref name="TOther"/>.</returns>
        IAsyncTask<TOther> AsIAsyncTask<TOther>(Func<T, TOther> taskResultConversionFunction);
    }
}