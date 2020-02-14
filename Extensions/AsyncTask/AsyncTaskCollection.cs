using System;
using System.Collections.Generic;
using System.Linq;

namespace Innoactive.Hub.SDK
{
    /// <inheritdoc cref="AsyncTask" />
    /// <summary>
    /// A collection of asynchronous tasks that will run in parallel and will invoke <seealso cref="IAsyncTask.OnFinished(Action)" /> / <seealso cref="IAsyncTask.Finished"/> as soon as all
    /// tasks of the collection have finished or <seealso cref="IAsyncTask.OnError" /> / <seealso cref="IAsyncTask.Errored"/> if one of the tasks fails.
    /// Also, <seealso cref="IAsyncTask.OnProgress"/> / <seealso cref="IAsyncTask.ProgressUpdate"/> are used to notify any listeners of progress
    /// </summary>
    public class AsyncTaskCollection : AsyncTask, IAsyncTask<object[]>
    {
        /// <inheritdoc />
        /// <summary>
        /// Event emitted when this task is completed, contains the result of the task
        /// </summary>
        public new event EventHandler<AsyncTaskFinishedResultEventArgs<object[]>> FinishedWithResult;

        /// <inheritdoc />
        /// <summary>
        /// The task's typed result (only available after the task has successfully run).
        /// Please rather use <see cref="OnFinished"/> or <seealso cref="FinishedWithResult"/> to access the result!
        /// </summary>
        public new object[] Result { get; protected set; }

        #region Constructors
        /// <inheritdoc />
        /// <summary>
        /// Default constructor allows to add tasks later on to the collection
        /// </summary>
        public AsyncTaskCollection()
        {
            Tasks = new List<IAsyncTask>();
        }

        /// <inheritdoc />
        /// <summary>
        /// Initialize a new task collection with a given array of tasks
        /// </summary>
        /// <param name="tasks"></param>
        public AsyncTaskCollection(params IAsyncTask[] tasks)
        {
            Tasks = new List<IAsyncTask>(tasks);
        }
        #endregion

        /// <inheritdoc />
        /// <summary>
        /// Marks the task as finished and executes all registered handlers for the <seealso cref="FinishedWithResult"/> event
        /// </summary>
        /// <param name="onFinished">the callback action to be run upon task completion</param>
        /// <returns>the instance of the task to easily chain method calls</returns>
        public IAsyncTask<object[]> OnFinished(Action<object[]> onFinished)
        {
            // convert the given action to an eventhandler and call it as soon as the task finishes
            EventHandler<AsyncTaskFinishedResultEventArgs<object[]>> finishedEventHandler = (sender, args) => onFinished?.Invoke(args.Result);
            FinishedWithResult += finishedEventHandler;
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Registers an error handler for any errors occuring during task execution
        /// </summary>
        /// <param name="onError"></param>
        /// <returns>the instance of the task to easily chain method calls</returns>
        public new IAsyncTask<object[]> OnError(Action<Exception> onError)
        {
            return (IAsyncTask<object[]>)base.OnError(onError);
        }

        /// <inheritdoc />
        /// <summary>
        /// Registers a progress handler for progress updates during task execution
        /// </summary>
        /// <param name="onProgress"></param>
        /// <returns>the instance of the task to easily chain method calls</returns>
        public new IAsyncTask<object[]> OnProgress(Action<float> onProgress)
        {
            return (IAsyncTask<object[]>)base.OnProgress(onProgress);
        }

        /// <inheritdoc />
        /// <summary>
        /// Marks the task as finished with the given result and executes all registered handlers for the <seealso cref="FinishedWithResult"/> event
        /// </summary>
        /// <param name="results">the result with which to complete the task</param>
        public void InvokeOnFinished(object[] results)
        {
            // Actually remember the typed result
            Result = results;

            // execute any base logic
            base.InvokeOnFinished(results);
        }

        /// <inheritdoc />
        /// <summary>
        /// Converts the result of this asynchronous task to the given <typeparamref name="TOther"/> using the provided <paramref name="taskResultConversionFunction"/>
        /// </summary>
        /// <typeparam name="TOther">the desired type of result</typeparam>
        /// <param name="taskResultConversionFunction">The function used to convert the result of type object[] to <typeparamref name="TOther"/></param>
        /// <returns>An async task that will return a result of the desired type <typeparamref name="TOther"/></returns>
        public IAsyncTask<TOther> AsIAsyncTask<TOther>(Func<object[], TOther> taskResultConversionFunction)
        {
            return AsyncTask<object[]>.ConvertTaskResultType(this, taskResultConversionFunction);
        }

        /// <inheritdoc />
        /// <summary>
        /// Marks the task as finished and executes all registered handlers for the <seealso cref="IAsyncTask.Finished" /> event
        /// </summary>
        public override void InvokeOnFinished()
        {
            // base logic
            base.InvokeOnFinished();

            // notify anyone about the available results
            FinishedWithResult?.Invoke(this, new AsyncTaskFinishedResultEventArgs<object[]>(Result));
        }

        /// <inheritdoc />
        /// <summary>
        /// Starts executing all tasks of the collection (in parallel)
        /// </summary>
        /// <returns>the disposable that can be used to dispose the entire collection of tasks</returns>
        public override IDisposable Execute()
        {
            // mark the task collection as running and notify everyone about it
            SetStarted();

            if (Count == 0)
            {
                FinalizeTaskCollection();
            }
            else
            {
                // for each provided async task, make sure to mark it as done in its OnFinished method
                // When all tasks are done, return their results
                // when one task throws an error, forward this error to anyone who cares
                foreach (IAsyncTask task in Tasks)
                {
                    task
                        .OnFinished(() =>
                        {
                        // when all tasks are finished, call the finished handler
                        if (Tasks.All(asyncTask => asyncTask.IsFinished))
                            {
                                FinalizeTaskCollection();
                            }
                        })
                        .OnProgress(f => InvokeOnProgress(Tasks.Average(asyncTask => asyncTask.Progress)))
                        .OnError(InvokeOnError)
                        .Execute();
                }
            }

            return new NoopDisposable();
        }

        /// <summary>
        /// Called when all tasks have been finished, use this to notify anyone of the finished list of tasks and providing the correct list of results
        /// </summary>
        protected virtual void FinalizeTaskCollection()
        {
            InvokeOnFinished(Tasks.Select(asyncTask => asyncTask.Result).ToArray());
        }

        /// <inheritdoc />
        /// <summary>
        /// Actually takes care of aborting the running task by e.g. disposing the task's handle
        /// Can be overridden in subclasses to allow different logic
        /// </summary>
        protected override void AbortTask()
        {
            // abort all running tasks individually
            foreach (IAsyncTask asyncTask in Tasks)
            {
                // try aborting, if the task has already finished or errored or has been already aborted, ignore!
                try
                {
                    asyncTask.Abort();
                }
                catch (TaskNotRunningException)
                {
                    // ignored
                }
            }
        }

        #region IAsyncTaskCollection
        /// <summary>
        /// collection of all asynchronous tasks that are to be executed in parallel
        /// </summary>
        protected ICollection<IAsyncTask> Tasks;

        /// <summary>
        /// Adds a task to the collection
        /// </summary>
        /// <param name="task"></param>
        public virtual void Add(IAsyncTask task)
        {
            if (Status != AsyncTaskStatus.Created)
            {
                throw new InvalidOperationException("Cannot add Tasks to this collection, since it has already been started");
            }

            Tasks.Add(task);
        }

        /// <summary>
        /// Removes the specified task from the collection
        /// </summary>
        /// <param name="task"></param>
        public void Remove(IAsyncTask task)
        {
            if (Status != AsyncTaskStatus.Created)
            {
                throw new InvalidOperationException("Cannot remove Tasks from this collection, since it has already been started");
            }

            Tasks.Remove(task);
        }

        /// <summary>
        /// Number of tasks in this collection.
        /// </summary>
        public int Count
        {
            get { return Tasks.Count; }
        }

        #endregion
    }

    /// <inheritdoc cref="IAsyncTask{T}" />
    /// <summary>
    /// A collection of asynchronous tasks that will run in parallel and will invoke <seealso cref="IAsyncTask{T}.OnFinished(System.Action{T})" /> as soon as all
    /// tasks of the collection have finished or <seealso cref="IAsyncTask{T}.OnError(Action{System.Exception})" /> if one of the tasks fails.
    /// </summary>
    /// <typeparam name="T">The type of result each single task of the collection will yield.</typeparam>
    /// <remarks>All tasks of this collection must be of type <seealso cref="IAsyncTask{T}"/> and use the same type parameter <typeparamref name="T"/></remarks>
    public class AsyncTaskCollection<T> : AsyncTaskCollection, IAsyncTask<T[]>
    {
        /// <inheritdoc />
        /// <summary>
        /// Event emitted when this task is completed, contains the result of the task
        /// </summary>
        public new event EventHandler<AsyncTaskFinishedResultEventArgs<T[]>> FinishedWithResult;

        /// <inheritdoc />
        /// <summary>
        /// The task's typed result (only available after the task has successfully run).
        /// Please rather use <see cref="OnFinished"/> or <seealso cref="FinishedWithResult"/> to access the result!
        /// </summary>
        public new T[] Result { get; protected set; }

        #region Constructors
        /// <inheritdoc />
        /// <summary>
        /// Default constructor allows to add tasks later on to the collection
        /// </summary>
        public AsyncTaskCollection() : base() { }

        /// <inheritdoc />
        /// <summary>
        /// Initialize a new task collection with a given array of tasks
        /// </summary>
        /// <param name="tasks"></param>
        public AsyncTaskCollection(params IAsyncTask<T>[] tasks) : this()
        {
            // convert all typed async tasks to non-typed ones and add them to the collection of tasks
            foreach (IAsyncTask<T> asyncTask in tasks)
            {
                Tasks.Add(asyncTask);
            }
        }
        #endregion

        /// <inheritdoc />
        /// <summary>
        /// Marks the task as finished and executes all registered handlers for the <seealso cref="FinishedWithResult"/> event
        /// </summary>
        /// <param name="onFinished">the callback action to be run upon task completion</param>
        /// <returns>the instance of the task to easily chain method calls</returns>
        public IAsyncTask<T[]> OnFinished(Action<T[]> onFinished)
        {
            // convert the given action to an eventhandler and call it as soon as the task finishes
            EventHandler<AsyncTaskFinishedResultEventArgs<T[]>> finishedEventHandler = (sender, args) => onFinished?.Invoke(args.Result);
            FinishedWithResult += finishedEventHandler;
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Registers a new callback to be executed when an error occurs while executing the task.
        /// </summary>
        /// <param name="onError">the callback action to be run upon an exception in the task</param>
        /// <returns>the instance of the task to easily chain method calls</returns>
        public new IAsyncTask<T[]> OnError(Action<Exception> onError)
        {
            return (IAsyncTask<T[]>)base.OnError(onError);
        }

        /// <inheritdoc />
        /// <summary>
        /// Registers a new callback to be executed whenever the progress (0...1) of the task execution changes.
        /// Can be used e.g. to visualize the progress as a loading bar
        /// </summary>
        /// <param name="onProgress">the callback action to be run upon progress updates</param>
        /// <returns>the instance of the task to easily chain method calls</returns>
        public new IAsyncTask<T[]> OnProgress(Action<float> onProgress)
        {
            return (IAsyncTask<T[]>)base.OnProgress(onProgress);
        }

        /// <inheritdoc />
        /// <summary>
        /// Marks the task as finished with the given result and executes all registered handlers for the <seealso cref="FinishedWithResult"/> event
        /// </summary>
        /// <param name="results">the typed result with which to complete the task</param>
        public void InvokeOnFinished(T[] results)
        {
            // save the typed result
            Result = results;

            // base logic (non-generic class)
            base.InvokeOnFinished(results);

            // notify anyone about the available result
            FinishedWithResult?.Invoke(this, new AsyncTaskFinishedResultEventArgs<T[]>(Result));
        }

        /// <inheritdoc />
        /// <summary>
        /// Adds a task to the collection
        /// </summary>
        /// <param name="task"></param>
        /// <remarks>Ensures that the task is of type <seealso cref="IAsyncTask{T}"/></remarks>
        public override void Add(IAsyncTask task)
        {
            // make sure only instances of IAsyncTask<T> are added
            IAsyncTask<T> typedTask = task as IAsyncTask<T>;
            if (typedTask == null)
            {
                throw new InvalidTaskResultTypeException($"Only tasks with a result type of {typeof(T)} can be added to this collection!");
            }

            base.Add(typedTask);
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
        protected override void FinalizeTaskCollection()
        {
            InvokeOnFinished(Tasks.Select(task => ((IAsyncTask<T>)task).Result).ToArray());
        }
    }
}
