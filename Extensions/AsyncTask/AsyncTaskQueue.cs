using System;
using System.Collections.Generic;
using System.Linq;

namespace Innoactive.Hub.SDK
{
    /// <inheritdoc />
    /// <summary>
    /// A queue of asynchronous tasks that will run in sequence and will invoke <see cref="IAsyncTask.OnFinished(System.Action)" /> / <see cref="IAsyncTask.Finished"/> as soon as the last task
    /// in the queue has finished or <see cref="IAsyncTask.OnError(System.Action{System.Exception})" /> / <see cref="IAsyncTask.Errored"/> if one of the tasks in the queue fails. The queue will also emit
    /// Progress event (<see cref="IAsyncTask.OnProgress" /> / <see cref="IAsyncTask.ProgressUpdate" /> )during task execution.
    /// </summary>
    public class AsyncTaskQueue : AsyncTaskCollection
    {
        #region Constructors

        /// <summary>
        /// Default constructor that allows to enqueue tasks later on.
        /// </summary>
        public AsyncTaskQueue() : base()
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Initialize a new task queue with a given array of tasks (in the given order)
        /// </summary>
        /// <param name="tasks"></param>
        public AsyncTaskQueue(params IAsyncTask[] tasks) : base(tasks)
        {
        }

        #endregion

        /// <inheritdoc />
        /// <summary>
        /// Actually runs the delegate and returns its result (an IDisposable object)
        /// that is to be disposed only after the OnFinished or OnError callback has
        /// been executed
        /// </summary>
        /// <returns>the result of the Task's delegate</returns>
        public override IDisposable Execute()
        {
            if (Tasks.Count == 0)
            {
                // no tasks, raise an error.
                throw new MissingTaskException("No list of tasks given!");
            }

            // mark the task queue as running and notify everyone about it
            SetStarted();

            // dequeue an item, wait until the task's finished, then dequeue the next one
            IEnumerator<IAsyncTask> enumerator = Tasks.GetEnumerator();
            return RunTextTask(enumerator);
        }

        /// <summary>
        /// Enqueues a new task
        /// </summary>
        /// <param name="task"></param>
        public virtual void Enqueue(IAsyncTask task)
        {
            if (Status != AsyncTaskStatus.Created)
            {
                throw new InvalidOperationException("Cannot enqueue tasks anymore, since the queue has already been started");
            }

            Add(task);
        }

        /// <summary>
        /// Called when all tasks have been finished, use this to notify anyone of the finished list of tasks and providing the correct list of results
        /// </summary>
        protected virtual void FinalizeTaskQueue()
        {
            InvokeOnFinished(Tasks.Select(asyncTask => asyncTask.Result).ToArray());
        }

        private IDisposable RunTextTask(IEnumerator<IAsyncTask> taskEnumerator)
        {
            if (taskEnumerator.MoveNext() == false)
            {
                // no more tasks to dequeue, we're finished
                FinalizeTaskQueue();
                return new NoopDisposable();
            }

            IAsyncTask task = taskEnumerator.Current;
            return task?.OnFinished(() =>
                {
                    // start the next task (unless there is no more)
                    RunTextTask(taskEnumerator);
                })
                .OnProgress(f =>
                {
                    InvokeOnProgress(Tasks.Average(asyncTask => asyncTask.Progress));
                })
                .OnError(InvokeOnError)
                .Execute();
        }
    }

    /// <inheritdoc cref="AsyncTaskQueue" />
    /// <summary>
    /// Generic version of <seealso cref="AsyncTaskQueue"/> that will return a list of results from the tasks run as a queue in the handlers attached via <see cref="OnFinished"/> / <see cref="FinishedWithResult"/>.
    /// Also emits progress updates to the handlers attached via <see cref="IAsyncTask.OnProgress"/> and <see cref="IAsyncTask.ProgressUpdate"/>
    /// </summary>
    public class AsyncTaskQueue<T> : AsyncTaskQueue, IAsyncTask<T[]>
    {
        /// <summary>
        /// Event emitted when this task is completed, contains the results of all tasks in the collection.
        /// </summary>
        public new event EventHandler<AsyncTaskFinishedResultEventArgs<T[]>> FinishedWithResult;

        /// <inheritdoc />
        /// <summary>
        /// The task's typed result (only available after the task has successfully run).
        /// Please rather use <see cref="OnFinished"/> or <seealso cref="FinishedWithResult"/> to access the result!
        /// </summary>
        public new T[] Result { get; private set; }

        #region Constructors

        /// <summary>
        /// Default constructor that allows to enqueue tasks later on.
        /// </summary>
        public AsyncTaskQueue() : base()
        {
        }

        /// <summary>
        /// Initialize a new task queue with a given array of tasks (in the given order).
        /// </summary>
        /// <param name="tasks"></param>
        public AsyncTaskQueue(params IAsyncTask<T>[] tasks) : base()
        {
            foreach (IAsyncTask<T> asyncTask in tasks)
            {
                Tasks.Add(asyncTask);
            }
        }

        #endregion

        /// <summary>
        /// Marks the task as finished and executes all registered handlers for the <seealso cref="FinishedWithResult"/> event.
        /// </summary>
        /// <param name="onFinished">The callback action to be run upon completion of the task collection.</param>
        /// <returns>The instance of the task to easily chain method calls.</returns>
        public IAsyncTask<T[]> OnFinished(Action<T[]> onFinished)
        {
            // Convert the given action to an event handler and call it as soon as the task finishes.
            EventHandler<AsyncTaskFinishedResultEventArgs<T[]>> finishedEventHandler = (sender, args) => onFinished?.Invoke(args.Result);
            FinishedWithResult += finishedEventHandler;
            return this;
        }

        /// <summary>
        /// Registers a new callback to be executed when an error occurs in any of the tasks of the queue.
        /// </summary>
        /// <param name="onError">The callback action to be run upon an exception in the task.</param>
        /// <returns>The instance of the task to easily chain method calls.</returns>
        public new IAsyncTask<T[]> OnError(Action<Exception> onError)
        {
            return (IAsyncTask<T[]>)base.OnError(onError);
        }

        /// <summary>
        /// Registers a new callback to be executed whenever the progress (0...1) of the task execution changes.
        /// Can be used e.g. to visualize the progress as a loading bar.
        /// </summary>
        /// <param name="onProgress">The callback action to be run upon progress updates.</param>
        /// <returns>The instance of the task to easily chain method calls.</returns>
        public new IAsyncTask<T[]> OnProgress(Action<float> onProgress)
        {
            return (IAsyncTask<T[]>)base.OnProgress(onProgress);
        }

        /// <inheritdoc />
        /// <summary>
        /// Marks the queue of tasks as finished with the given results and executes all registered handlers for the <seealso cref="FinishedWithResult"/> event.
        /// </summary>
        /// <param name="results">the typed result with which to complete the task</param>
        public void InvokeOnFinished(T[] results)
        {
            // also save the typed result
            Result = results;

            // base logic (non-generic class)
            base.InvokeOnFinished();

            // invoke the typed success handlers
            FinishedWithResult?.Invoke(this, new AsyncTaskFinishedResultEventArgs<T[]>(Result));
        }

        /// <inheritdoc />
        /// <summary>
        /// Enqueues a new task
        /// </summary>
        /// <param name="task"></param>
        public override void Enqueue(IAsyncTask task)
        {
            // make sure only instances of IAsyncTask<T> are added
            IAsyncTask<T> typedTask = task as IAsyncTask<T>;
            if (typedTask == null)
            {
                throw new InvalidTaskResultTypeException($"Only tasks with a result type of {typeof(T)} can be added to this collection!");
            }

            base.Enqueue(task);
        }

        /// <inheritdoc />
        /// <summary>
        /// Converts the result of this asynchronous task to the given <typeparamref name="TOther"/> using the provided <paramref name="taskResultConversionFunction"/>
        /// </summary>
        /// <typeparam name="TOther">the desired type of result</typeparam>
        /// <param name="taskResultConversionFunction">The function used to convert the result of type <typeparamref name="T"/> to <typeparamref name="TOther"/></param>
        /// <returns>An async task that will return a result of the desired type <typeparamref name="TOther"/></returns>
        public IAsyncTask<TOther> AsIAsyncTask<TOther>(Func<T[], TOther> taskResultConversionFunction)
        {
            return AsyncTask<T[]>.ConvertTaskResultType(this, taskResultConversionFunction);
        }

        /// <inheritdoc />
        /// <summary>
        /// Called when all tasks have been finished, use this to notify anyone of the finished list of tasks and providing the correct list of results
        /// </summary>
        protected override void FinalizeTaskQueue()
        {
            InvokeOnFinished(Tasks.Select(asyncTask => ((IAsyncTask<T>)asyncTask).Result).ToArray());
        }
    }
}