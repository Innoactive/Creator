using System;
using System.Collections;
using System.Collections.Generic;
using Innoactive.Hub.Training.Unity.Utils;
using UnityEngine;

namespace Innoactive.Hub.Training.Utils.Timer
{
    /// <summary>
    /// Manages the operation of timers implementing <see cref="ITimer"/> so they can be all controlled centrally. Also allows timers to access Unity coroutines.
    /// </summary>
    public class TimerManager : UnitySceneSingleton<TimerManager>
    {
        private List<ITimer> registeredTimers = new List<ITimer>();

        /// <summary>
        /// Creates and returns an <see cref="ITimer"/>.
        /// </summary>
        public ITimer CreateTimer(float timeout)
        {
            UnityTimer timer = new UnityTimer(timeout);
            registeredTimers.Add(timer);
            return timer;
        }

        /// <summary>
        /// Pauses the operation of all timers currently registered to <see cref="TimerManager"/>.
        /// </summary>
        public void PauseAllTimers()
        {
            foreach (ITimer timer in registeredTimers)
            {
                timer.Pause();
            }
        }

        /// <summary>
        /// Resumes the operation of all timers currently registered to <see cref="TimerManager"/>.
        /// </summary>
        public void ResumeAllTimers()
        {
            foreach (ITimer timer in registeredTimers)
            {
                timer.Resume();
            }
        }

        /// <summary>
        /// Stops the operation of all timers currently registered to <see cref="TimerManager"/>.
        /// </summary>
        public void StopAllTimers()
        {
            foreach (ITimer timer in registeredTimers)
            {
                timer.Stop();
            }
        }

        /// <summary>
        /// Removes all timers from the registered timers list so they cannot be controlled by <see cref="TimerManager"/> any more.
        /// </summary>
        public void DisposeAllTimers()
        {
            List<ITimer> timers = new List<ITimer>(registeredTimers);
            foreach (ITimer timer in timers)
            {
                timer.Dispose();
            }
        }

        /// <summary>
        /// Removes given timer from the registered list so it cannot be managed by <see cref="TimerManager"/>.
        /// </summary>
        private void RemoveTimer(ITimer timer)
        {
            registeredTimers.Remove(timer);
        }

        protected override void OnDestroy()
        {
            StopAllCoroutines();
        }

        /// <summary>
        /// Unity based timer implementation using coroutines. Can only be created by <see cref="TimerManager"/> and accessed via <see cref="ITimer"/>
        /// </summary>
        private class UnityTimer : ITimer
        {
            private readonly float interval;
            private float startTime;
            private float elapsedTimeBeforePause;
            private Coroutine activeCoroutine;

            /// <inheritdoc />
            public TimerState CurrentState { private set; get; }

            /// <inheritdoc />
            public bool AutoReset { set; get; }

            /// <inheritdoc />
            public event EventHandler<TimerEventArgs> Elapsed;

            /// <param name="interval">Interval in seconds until Elapsed event is raised.</param>
            public UnityTimer(float interval)
            {
                this.interval = interval;
                CurrentState = TimerState.Stopped;
            }

            /// <inheritdoc />
            public void Start()
            {
                Stop();
                activeCoroutine = Instance.StartCoroutine(ActivateTimer(interval));
            }

            /// <inheritdoc />
            public void Stop()
            {
                DeactivateTimer();
                elapsedTimeBeforePause = 0;
                CurrentState = TimerState.Stopped;
            }

            /// <inheritdoc />
            public void Pause()
            {
                if (CurrentState == TimerState.Running && DeactivateTimer())
                {
                    elapsedTimeBeforePause = ElapsedTime();
                    CurrentState = TimerState.Paused;
                }
            }

            /// <inheritdoc />
            public void Resume()
            {
                if (CurrentState == TimerState.Paused)
                {
                    float remainingTime = interval - elapsedTimeBeforePause;
                    activeCoroutine = Instance.StartCoroutine(ActivateTimer(remainingTime));
                }
            }

            /// <inheritdoc />
            public float ElapsedTime()
            {
                switch (CurrentState)
                {
                    case TimerState.Paused:
                        return elapsedTimeBeforePause;
                    case TimerState.Running:
                        return (Time.time - startTime) + elapsedTimeBeforePause;
                    case TimerState.Finished:
                        return interval;
                    case TimerState.Stopped:
                    default:
                        return 0;
                }
            }

            /// <inheritdoc />
            public void Dispose()
            {
                Stop();
                Instance.RemoveTimer(this);
            }

            private bool DeactivateTimer()
            {
                if (activeCoroutine != null)
                {
                    Instance.StopCoroutine(activeCoroutine);
                    activeCoroutine = null;
                    return true;
                }

                return false;
            }

            private IEnumerator ActivateTimer(float timerInterval)
            {
                startTime = Time.time;
                CurrentState = TimerState.Running;
                yield return new WaitForSeconds(timerInterval);

                if (Elapsed != null)
                {
                    Elapsed.Invoke(this, new TimerEventArgs());
                }

                if (AutoReset)
                {
                    Start();
                }
                else
                {
                    CurrentState = TimerState.Finished;
                }
            }
        }
    }
}
