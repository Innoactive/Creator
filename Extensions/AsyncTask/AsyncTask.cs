using System;

namespace Innoactive.Hub.SDK
{
    /// <inheritdoc />
    /// <summary>
    /// Asynchronous task providing an interface that allows to register handlers for the finished task.
    /// This task does not provide any results when finished, if you need results, please <seealso cref="IAsyncTask{T}" />
    /// (<see cref="IAsyncTask{T}.OnFinished(Action{T})" />) as well as it's progress updates (<see cref="IAsyncTask.OnProgress" />) and
    /// errors (<see cref="IAsyncTask.OnError" />
    /// </summary>
    public class AsyncTask : IAsyncTask
    {
        /// <summary>
        /// Exception when a <see cref="IAsyncTask"/> is executed without a valid task to execute.
        /// </summary>
        public class MissingTaskException : Exception
        {
            /// <summary>
            /// Creates an instance that reports <paramref name="message"/> as exception message.
            /// </summary>
            public MissingTaskException(string message) : base(message)
            {
            }
        }

        /// <summary>
        /// Exception when a <see cref="IAsyncTask"/> is executed without a valid action as <see cref="AsyncTask.TaskDelegate"/>.
        /// </summary>
        public class MissingTaskDelegateException : Exception
        {
            /// <summary>
            /// Creates an instance that reports <paramref name="message"/> as exception message.
            /// </summary>
            public MissingTaskDelegateException(string message) : base(message)
            {
            }
        }

        /// <summary>
        /// Exception when a <see cref="IAsyncTask"/> is executed while it is already running.
        /// </summary>
        public class TaskAlreadyRunningException : Exception
        {
            /// <summary>
            /// Creates an instance that reports <paramref name="message"/> as exception message.
            /// </summary>
            public TaskAlreadyRunningException(string message) : base(message)
            {
            }
        }

        /// <summary>
        /// Exception when an <see cref="IAsyncTask"/>'s methods are called that require a running task,
        /// but the task has not started yet.
        /// </summary>
        public class TaskNotRunningException : Exception
        {
            /// <summary>
            /// Creates an instance that reports <paramref name="message"/> as exception message.
            /// </summary>
            public TaskNotRunningException(string message) : base(message)
            {
            }
        }

        /// <summary>
        /// Exception when a <see cref="IAsyncTask"/> receives a finish signal with a wrong result type.
        /// </summary>
        public class InvalidTaskResultTypeException : Exception
        {
            /// <summary>
            /// Creates an instance that reports <paramref name="message"/> as exception message.
            /// </summary>
            public InvalidTaskResultTypeException(string message) : base(message)
            {
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Event emitted whenever the progress of the task changes
        /// </summary>
        public event EventHandler<AsyncTaskProgressEventArgs> ProgressUpdate;

        /// <inheritdoc />
        /// <summary>
        /// Event emitted when this task has been started
        /// </summary>
        public event EventHandler<AsyncTaskStartedEventArgs> Started;

        /// <inheritdoc />
        /// <summary>
        /// Event emitted when this task is completed
        /// </summary>
        public event EventHandler<AsyncTaskFinishedEventArgs> Finished;

        /// <inheritdoc />
        /// <summary>
        /// Event emitted when this task is completed, contains the result of the task
        /// </summary>
        public event EventHandler<AsyncTaskFinishedResultEventArgs<object>> FinishedWithResult;

        /// <inheritdoc />
        /// <summary>
        /// Event emitted when this task has errored
        /// </summary>
        public event EventHandler<AsyncTaskErrorEventArgs> Errored;

        /// <inheritdoc />
        /// <summary>
        /// Event emitted when this task has errored
        /// </summary>
        public event EventHandler<AsyncTaskAbortedEventArgs> Aborted;

        /// <inheritdoc />
        /// <summary>
        /// The task's typed result (only available after the task has successfully run).
        /// Please rather use <see cref="OnFinished(System.Action)"/> or <seealso cref="FinishedWithResult"/> to access the result!
        /// </summary>
        public object Result { get; protected set; }

        /// <inheritdoc />
        /// <summary>
        /// The task's current progress (ranges from 0 to 1)
        /// </summary>
        public float Progress { get; protected set; }

        /// <inheritdoc />
        public bool IsFinished
        {
            get { return Status == AsyncTaskStatus.Finished; }
        }

        /// <inheritdoc />
        public bool IsRunning
        {
            get { return Status == AsyncTaskStatus.Running; }
        }

        /// <inheritdoc />
        public bool IsErrored
        {
            get { return Status == AsyncTaskStatus.Errored; }
        }

        /// <inheritdoc />
        public bool IsAborted
        {
            get { return Status == AsyncTaskStatus.Aborted; }
        }

        /// <inheritdoc />
        /// <summary>
        /// The actual workload that the task will performed
        /// </summary>
        [Obsolete("Setting the task's delegate via the property is no longer supported. Please use the Task's constructor instead!")]
        public Func<IDisposable> TaskDelegate
        {
            set { Action = task => value.Invoke(); }
        }

        /// <summary>
        /// The task's current status
        /// </summary>
        protected AsyncTaskStatus Status { get; set; }

        /// <summary>
        /// The actual workload that the task will performed
        /// </summary>
        protected Func<IAsyncTask, IDisposable> Action { get; set; }

        /// <summary>
        /// A handle to the task's delegate. Can be used to dispose the task's delegate
        /// </summary>
        protected IDisposable TaskDelegateHandle { get; set; }

        #region Constructors

        /// <summary>
        /// Default constructor, takes care of initializing handler (finished, progress, error) lists
        /// </summary>
        [Obsolete("The parameterless default constructor is deprecated and will be removed in a future version!")]
        public AsyncTask()
        {
            Status = AsyncTaskStatus.Created;
        }

        /// <inheritdoc />
        /// <summary>
        /// Public constructor expecting the task's delegate (the actual operation)
        /// </summary>
        /// <param name="action"></param>
        public AsyncTask(Func<IAsyncTask, IDisposable> action) : this()
        {
            Action = action;
        }

        #endregion

        /// <inheritdoc />
        /// <summary>
        /// Registers a new callback to be executed as soon as the task has finished.
        /// </summary>
        /// <param name="onFinished">the callback action to be run upon task completion</param>
        /// <returns>the instance of the task to easily chain method calls</returns>
        public IAsyncTask OnFinished(Action onFinished)
        {
            // convert the given action to an EventHandler and call it as soon as the task finishes
            EventHandler<AsyncTaskFinishedEventArgs> finishedEventHandler = (sender, args) => onFinished?.Invoke();
            Finished += finishedEventHandler;
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Marks the task as finished and executes all registered handlers for the <seealso cref="FinishedWithResult"/> event
        /// </summary>
        /// <param name="onFinished">the callback action to be run upon task completion</param>
        /// <returns>the instance of the task to easily chain method calls</returns>
        public IAsyncTask OnFinished(Action<object> onFinished)
        {
            // convert the given action to an EventHandler and call it as soon as the task finishes
            EventHandler<AsyncTaskFinishedResultEventArgs<object>> finishedEventHandler = (sender, args) => onFinished?.Invoke(args.Result);
            FinishedWithResult += finishedEventHandler;
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Registers an error handler for any errors occurring during task execution
        /// </summary>
        /// <param name="onError"></param>
        /// <returns>the instance of the task to easily chain method calls</returns>
        public IAsyncTask OnError(Action<Exception> onError)
        {
            // convert the given action to an eventhandler and call it as soon as the task errors
            EventHandler<AsyncTaskErrorEventArgs> errorEventHandler = (sender, args) => onError?.Invoke(args.Exception);
            Errored += errorEventHandler;
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Registers a progress handler for progress updates during task execution
        /// </summary>
        /// <param name="onProgress"></param>
        /// <returns>the instance of the task to easily chain method calls</returns>
        public IAsyncTask OnProgress(Action<float> onProgress)
        {
            // convert the given action to an EventHandler and call it as soon as progress updates occur
            EventHandler<AsyncTaskProgressEventArgs> progressEventHandler = (sender, args) => onProgress?.Invoke(args.Progress);
            ProgressUpdate += progressEventHandler;
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Actually runs the delegate and returns its result (an IDisposable object)
        /// that is to be disposed only after the OnFinished or OnError callback has
        /// been executed
        /// </summary>
        /// <returns>the result of the Task's delegate</returns>
        public virtual IDisposable Execute()
        {
            // if we (still) don't have an actual delegate for the task, throw an exception right away
            if (HasValidDelegate() == false)
            {
                throw new MissingTaskDelegateException("The task cannot be executed without a delegate.");
            }

            if (IsRunning)
            {
                throw new TaskAlreadyRunningException("This task has already been started.");
            }

            try
            {
                // mark the task as running and notify everyone about it
                SetStarted();
                TaskDelegateHandle = Action?.Invoke(this) ?? new NoopDisposable();
            }
            catch (Exception exception)
            {
                InvokeOnError(exception);
            }

            return TaskDelegateHandle;
        }

        /// <inheritdoc />
        /// <summary>
        /// Abort this task's operation
        /// </summary>
        public virtual void Abort()
        {
            if (IsRunning == false)
            {
                throw new TaskNotRunningException("You are trying to abort a task that's not currently running");
            }

            // aborts / disposes the actually running task
            AbortTask();

            // mark the task as aborted
            Status = AsyncTaskStatus.Aborted;

            // notify everyone of the abortion
            Aborted?.Invoke(this, new AsyncTaskAbortedEventArgs());
        }

        /// <inheritdoc />
        /// <summary>
        /// Marks the task as finished and executes all registered handlers for the <seealso cref="IAsyncTask.Finished" /> event
        /// </summary>
        public virtual void InvokeOnFinished()
        {
            MarkAsFinished();

            // Notify anyone about the available result.
            FinishedWithResult?.Invoke(this, new AsyncTaskFinishedResultEventArgs<object>(Result));

            // And notify everyone about the finished task.
            Finished?.Invoke(this, new AsyncTaskFinishedEventArgs());
        }

        /// <summary>
        /// Marks the task as Finished and sets the progress to 100%.
        /// </summary>
        protected void MarkAsFinished()
        {
            // mark the task as finished
            Status = AsyncTaskStatus.Finished;

            // set the progress to 1 (if it isn't already)
            if (Progress < 1)
            {
                InvokeOnProgress(1);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Marks the task as finished with the given result and executes all registered handlers for the <seealso cref="FinishedWithResult"/> event
        /// </summary>
        /// <param name="result">the result with which to complete the task</param>
        public void InvokeOnFinished(object result)
        {
            SetResult(result);

            // base logic (non-generic class)
            InvokeOnFinished();
        }

        /// <summary>
        /// Sets the outcome / result of this task.
        /// </summary>
        /// <param name="result">the result of the task</param>
        protected virtual void SetResult(object result)
        {
            Result = result;
        }

        /// <inheritdoc />
        /// <summary>
        /// Marks this task as errored and notifies all listeners (<seealso cref="IAsyncTask.Errored" />) of the provided exception that happened during task execution
        /// </summary>
        /// <param name="error">the exception that occurred while running the task</param>
        public virtual void InvokeOnError(Exception error)
        {
            // mark the task as errored
            Status = AsyncTaskStatus.Errored;

            // set the progress to 1 (if it isn't already)
            if (Progress < 1)
            {
                InvokeOnProgress(1);
            }

            // and notify everyone about the exception
            Errored?.Invoke(this, new AsyncTaskErrorEventArgs(error));
        }

        /// <inheritdoc />
        /// <summary>
        /// Updates the task's progress and notifies any listeners (<seealso cref="IAsyncTask.ProgressUpdate" />) of changed progress on the async task
        /// </summary>
        /// <param name="progress">the progress (0...1)</param>
        public void InvokeOnProgress(float progress)
        {
            // update the internal progress
            Progress = progress;

            // and notify everyone about the updated progress
            ProgressUpdate?.Invoke(this, new AsyncTaskProgressEventArgs(Progress));
        }

        /// <summary>
        /// Actually takes care of aborting the running task by e.g. disposing the task's handle
        /// Can be overridden in subclasses to allow different logic
        /// </summary>
        protected virtual void AbortTask()
        {
            TaskDelegateHandle.Dispose();
        }

        /// <summary>
        /// Marks this task as started and invokes the <see cref="Started"/> event
        /// </summary>
        protected void SetStarted()
        {
            Status = AsyncTaskStatus.Running;
            Started?.Invoke(this, new AsyncTaskStartedEventArgs());
        }

        /// <summary>
        /// Whether or not the Task's delegate is valid or not
        /// </summary>
        /// <returns></returns>
        protected virtual bool HasValidDelegate()
        {
            return Action != null;
        }

        #region Static Members

        /// <summary>
        /// Helper method that will return a task that only finished when all provided tasks have finished and
        /// will yield their results in the end
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static IAsyncTask WhenAll(params IAsyncTask[] tasks)
        {
            // intialize a new AsyncTaskCollection with all the given tasks
            return new AsyncTaskCollection(tasks);
        }

        #endregion
    }

    /// <inheritdoc cref="AsyncTask" />
    /// <summary>
    /// Asynchronous task providing an interface to register handlers for the final result of the async task
    /// (<see cref="OnFinished" />) as well as it's progress updates (<see cref="OnProgress" />) and
    /// errors (<see cref="OnError" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AsyncTask<T> : AsyncTask, IAsyncTask<T>
    {
        /// <inheritdoc />
        /// <summary>
        /// Event emitted when this task is completed, contains the result of the task
        /// </summary>
        public new event EventHandler<AsyncTaskFinishedResultEventArgs<T>> FinishedWithResult;

        /// <inheritdoc />
        /// <summary>
        /// The task's typed result (only available after the task has successfully run).
        /// Please rather use <see cref="OnFinished"/> or <seealso cref="FinishedWithResult"/> to access the result!
        /// </summary>
        public new T Result { get; protected set; }

        /// <summary>
        /// The actual workload that the task will perform
        /// </summary>
        protected new Func<IAsyncTask<T>, IDisposable> Action
        {
            set { base.Action = task => value.Invoke(this); }
        }

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Default constructor
        /// </summary>
        public AsyncTask() : base()
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Public constructor expecting the task's delegate (the actual operation)
        /// </summary>
        /// <param name="action"></param>
        public AsyncTask(Func<IAsyncTask<T>, IDisposable> action) : this()
        {
            Action = action;
        }

        #endregion

        /// <inheritdoc />
        /// <summary>
        /// Marks the task as finished and executes all registered handlers for the <seealso cref="FinishedWithResult"/> event
        /// </summary>
        /// <param name="onFinished">the callback action to be run upon task completion</param>
        /// <returns>the instance of the task to easily chain method calls</returns>
        public IAsyncTask<T> OnFinished(Action<T> onFinished)
        {
            // convert the given action to an eventhandler and call it as soon as the task finishes
            EventHandler<AsyncTaskFinishedResultEventArgs<T>> finishedEventHandler = (sender, args) => onFinished?.Invoke(args.Result);
            FinishedWithResult += finishedEventHandler;
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Registers a new callback to be executed when an error occurs while executing the task.
        /// </summary>
        /// <param name="onError">the callback action to be run upon an exception in the task</param>
        /// <returns>the instance of the task to easily chain method calls</returns>
        public new IAsyncTask<T> OnError(Action<Exception> onError)
        {
            return (IAsyncTask<T>)base.OnError(onError);
        }

        /// <inheritdoc />
        /// <summary>
        /// Registers a new callback to be executed whenever the progress (0...1) of the task execution changes.
        /// Can be used e.g. to visualize the progress as a loading bar
        /// </summary>
        /// <param name="onProgress">the callback action to be run upon progress updates</param>
        /// <returns>the instance of the task to easily chain method calls</returns>
        public new IAsyncTask<T> OnProgress(Action<float> onProgress)
        {
            return (IAsyncTask<T>)base.OnProgress(onProgress);
        }

        /// <inheritdoc />
        /// <summary>
        /// Marks the task as finished with the given result and executes all registered handlers for the <seealso cref="FinishedWithResult"/> event
        /// </summary>
        /// <param name="results">the typed result with which to complete the task</param>
        public virtual void InvokeOnFinished(T results)
        {
            SetResult(results);

            MarkAsFinished();

            // notify anyone about the available result
            FinishedWithResult?.Invoke(this, new AsyncTaskFinishedResultEventArgs<T>(Result));

            // Base logic (non-generic class).
            base.InvokeOnFinished();
        }

        /// <summary>
        /// Sets the (generic) outcome / result of this task.
        /// </summary>
        /// <param name="result">the result of the task</param>
        protected virtual void SetResult(T result)
        {
            // Save the typed result.
            Result = result;

            // Also save the untyped (object) result.
            SetResult((object)result);
        }

        /// <inheritdoc />
        /// <summary>
        /// Abort this task's operation
        /// </summary>
        public override void Abort()
        {
            base.Abort();
            Result = default(T);
        }

        /// <inheritdoc />
        /// <summary>
        /// Converts the result of this asynchronous task to the given <typeparamref name="TOther"/> using the provided <paramref name="taskResultConversionFunction"/>
        /// </summary>
        /// <typeparam name="TOther">the desired type of result</typeparam>
        /// <param name="taskResultConversionFunction">The function used to convert the result of type <typeparamref name="T"/> to <typeparamref name="TOther"/></param>
        /// <returns>An async task that will return a result of the desired type <typeparamref name="TOther"/></returns>
        public IAsyncTask<TOther> AsIAsyncTask<TOther>(Func<T, TOther> taskResultConversionFunction)
        {
            return ConvertTaskResultType(this, taskResultConversionFunction);
        }

        #region Static Members

        /// <summary>
        /// Helper method that will return a task that only finished when all provided tasks have finished and
        /// will yield their results in the end
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static IAsyncTask<T[]> WhenAll(params IAsyncTask<T>[] tasks)
        {
            // intialize a new AsyncTaskCollection with all the given tasks
            return new AsyncTaskCollection<T>(tasks);
        }

        /// <summary>
        /// Converts the result of this asynchronous task to the given <typeparamref name="TOther"/> using the provided <paramref name="taskResultConversionFunction"/>
        /// </summary>
        /// <typeparam name="TOther">the desired type of result</typeparam>
        /// <param name="originalTask">the task for which to convert the result type</param>
        /// <param name="taskResultConversionFunction">The function used to convert the result of type <typeparamref name="T"/> to <typeparamref name="TOther"/></param>
        /// <returns>An async task that will return a result of the desired type <typeparamref name="TOther"/></returns>
        public static IAsyncTask<TOther> ConvertTaskResultType<TOther>(IAsyncTask<T> originalTask, Func<T, TOther> taskResultConversionFunction)
        {
            AsyncTask<TOther> conversionTask = new AsyncTask<TOther>(task =>
            {
                originalTask.OnFinished(obj =>
                {
                    try
                    {
                        task.InvokeOnFinished(taskResultConversionFunction.Invoke(obj));
                    }
                    catch (Exception exception)
                    {
                        // catch any errors in the conversion function
                        task.InvokeOnError(exception);
                    }
                });
                originalTask.OnError(task.InvokeOnError);
                originalTask.OnProgress(task.InvokeOnProgress);
                return originalTask.Execute();
            });
            return conversionTask;
        }

        #endregion
    }
}
