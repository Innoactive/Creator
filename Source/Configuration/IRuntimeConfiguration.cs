using System;
using System.Collections.ObjectModel;
using Innoactive.Hub.Training.SceneObjects;
using Innoactive.Hub.Training.Configuration.Modes;
using UnityEngine;

namespace Innoactive.Hub.Training.Configuration
{
    public class ModeChangedEventArgs : EventArgs
    {
        public IMode Mode { get; private set; }

        public ModeChangedEventArgs(IMode mode)
        {
            Mode = mode;
        }
    }

    /// <summary>
    /// An interface for training runtime configurations. Implement it to create your own.
    /// </summary>
    public interface IRuntimeConfiguration
    {
        /// <summary>
        /// SceneObjectRegistry gathers all created TrainingSceneEntities.
        /// </summary>
        ISceneObjectRegistry SceneObjectRegistry { get; }

        /// <summary>
        /// Trainee scene object.
        /// </summary>
        TrainingSceneObject Trainee { get; }

        /// <summary>
        /// Default audio source to play audio from.
        /// </summary>
        AudioSource InstructionPlayer { get; }

        /// <summary>
        /// Returns the training entity state logger.
        /// It logs the activity of all activated entities in the console.
        /// </summary>
        ILogger EntityStateLogger { get; set; }

        /// <summary>
        /// Configuration of the training entity state logger.
        /// </summary>
        EntityStateLoggerConfig EntityStateLoggerConfig { get; set; }

        /// <summary>
        /// The ordered collection of all available training modes.
        /// </summary>
        ReadOnlyCollection<IMode> AvailableModes { get; }

        /// <summary>
        /// Returns the deserialized training course from <see cref="SelectedCourseStreamingAssetsPath"/>.
        /// </summary>
        ICourse LoadCourse();

        /// <summary>
        /// Set the current training mode.
        /// </summary>
        /// <param name="index">The index of the desired training mode.</param>
        void SetMode(int index);

        /// <summary>
        /// The current training mode.
        /// </summary>
        IMode GetCurrentMode();

        /// <summary>
        /// The index of the current training mode.
        /// </summary>
        int GetCurrentModeIndex();

        /// <summary>
        /// The event that has to be invoked in <see cref="SetMode"/> method.
        /// </summary>
        event EventHandler<ModeChangedEventArgs> ModeChanged;
    }
}
