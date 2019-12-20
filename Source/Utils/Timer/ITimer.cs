using System;

namespace Innoactive.Hub.Training.Utils.Timer
{
    public class TimerEventArgs : EventArgs { }

    /// <summary>
    /// Defines the possible states that the timer can be in
    /// </summary>
    public enum TimerState { Running, Paused, Stopped, Finished }

    public interface ITimer : IDisposable
    {
        /// <summary>
        /// Current state of the timer
        /// </summary>
        TimerState CurrentState { get; }

        /// <summary>
        /// Indicates if the timer is going to be restarted after each completion
        /// </summary>
        bool AutoReset { set; get; }

        /// <summary>
        /// Indicates if the timer is going to be restarted after each completion
        /// </summary>
        event EventHandler<TimerEventArgs> Elapsed;

        /// <summary>
        /// Starts the timer, if the timer is already running stops it first
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the timer if it's running and returns whether it successfully stopped or not
        /// </summary>
        void Stop();

        /// <summary>
        /// Pauses the timer
        /// </summary>
        void Pause();

        /// <summary>
        /// Resumes the timer if it is paused before
        /// </summary>
        void Resume();

        /// <summary>
        /// Returns the elapsed time since the timer has started
        /// </summary>
        float ElapsedTime();
    }
}
