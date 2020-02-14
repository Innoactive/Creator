using System;
using System.Collections.Generic;

namespace Innoactive.Hub.SDK
{
    /// <summary>
    /// AsyncTaskScheduler that limits the maximum number of concurrently running AsyncTasks. If more
    /// than <see cref="MaxConcurrentlyRunningTasks"/> are sent at the same time, all requests exceeding
    /// the limit will be queued and sent as soon as running requests finish.
    /// </summary>
    public class MaxConcurrentAsyncTaskScheduler : IAsyncTaskScheduler
    {
        /// <summary>
        /// Specifies the maximum number of AsyncTasks that may run concurrently.
        /// </summary>
        public int MaxConcurrentlyRunningTasks { get; set; }

        private bool CanStartAnotherTask
        {
            get { return (MaxConcurrentlyRunningTasks <= 0 || runningTasks.Count < MaxConcurrentlyRunningTasks); }
        }

        #region Private Members

        private HashSet<IAsyncTask> runningTasks = new HashSet<IAsyncTask>();

        // Using LinkedList instead of Queue to allow deletion of aborted tasks
        private LinkedList<IAsyncTask> queuedTasks = new LinkedList<IAsyncTask>();

        #endregion

        #region Private Helper Class

        private class ForwardTaskAbortion : IDisposable
        {
            private MaxConcurrentAsyncTaskScheduler parent;
            private readonly IAsyncTask requestTask;

            public ForwardTaskAbortion(MaxConcurrentAsyncTaskScheduler parent, IAsyncTask requestTask)
            {
                this.parent = parent;
                this.requestTask = requestTask;
            }

            public void Dispose()
            {
                parent.HandleScheduleTaskAbortion(requestTask);
            }
        }

        #endregion

        /// <summary>
        /// Creates a new instance and assigns <paramref name="maxConcurrentlyRunningRequests"/> to <see cref="MaxConcurrentlyRunningTasks"/>.
        /// </summary>
        public MaxConcurrentAsyncTaskScheduler(int maxConcurrentlyRunningRequests = 0)
        {
            MaxConcurrentlyRunningTasks = maxConcurrentlyRunningRequests;
        }

        /// <inheritdoc />
        public IAsyncTask Schedule(IAsyncTask requestTask)
        {
            IAsyncTask scheduleTask = new AsyncTask(task => PerformScheduling(requestTask));

            // Forward events from original task to scheduling task.
            requestTask.OnProgress(scheduleTask.InvokeOnProgress)
                .OnError(scheduleTask.InvokeOnError)
                .OnFinished(result => scheduleTask.InvokeOnFinished(result));

            return scheduleTask;
        }

        /// <inheritdoc />
        public IAsyncTask<T> Schedule<T>(IAsyncTask<T> requestTask)
        {
            IAsyncTask<T> scheduleTask = new AsyncTask<T>(task => PerformScheduling(requestTask));

            // Forward events from original task to scheduling task.
            requestTask.OnProgress(scheduleTask.InvokeOnProgress)
                .OnError(scheduleTask.InvokeOnError)
                .OnFinished(result => scheduleTask.InvokeOnFinished(result));

            return scheduleTask;
        }

        private IDisposable PerformScheduling(IAsyncTask requestTask)
        {
            // When finished, unregister as running,
            requestTask.OnError(exception => HandleTaskEnd(requestTask))
                .OnFinished(() => HandleTaskEnd(requestTask));
            requestTask.Aborted += (source, args) => HandleTaskEnd(requestTask);

            if (CanStartAnotherTask)
            {
                lock (runningTasks)
                {
                    runningTasks.Add(requestTask);
                }

                return requestTask.Execute();
            }
            else
            {
                lock (runningTasks)
                {
                    queuedTasks.AddLast(requestTask);
                }

                return new ForwardTaskAbortion(this, requestTask);
            }
        }

        private void HandleTaskEnd(IAsyncTask task)
        {
            lock (runningTasks)
            {
                runningTasks.Remove(task);
                while (CanStartAnotherTask && queuedTasks.Count > 0)
                {
                    // start next task in queue
                    IAsyncTask nextTask = queuedTasks.First.Value;
                    queuedTasks.RemoveFirst();
                    runningTasks.Add(nextTask);
                    nextTask.Execute();
                }
            }
        }

        private void HandleScheduleTaskAbortion(IAsyncTask requestTask)
        {
            lock (runningTasks)
            {
                runningTasks.Remove(requestTask);
                queuedTasks.Remove(requestTask);
            }

            if (requestTask.IsRunning)
            {
                requestTask.Abort();
            }
        }
    }
}