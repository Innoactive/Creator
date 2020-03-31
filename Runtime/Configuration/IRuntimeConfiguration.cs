using UnityEngine;
using System.Collections.ObjectModel;
using Innoactive.Creator.Core.Configuration.Modes;
using Innoactive.Creator.Core.SceneObjects;
using Innoactive.Creator.Core.Serialization;

namespace Innoactive.Creator.Core.Configuration
{
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
        /// Defines the serializer which should be used to serialize training courses.
        /// </summary>
        ICourseSerializer Serializer { get; set; }

        /// <summary>
        /// Returns the mode handler for the training.
        /// </summary>
        IModeHandler Modes { get; }

        /// <summary>
        /// Trainee scene object.
        /// </summary>
        TrainingSceneObject Trainee { get; }

        /// <summary>
        /// Default audio source to play audio from.
        /// </summary>
        AudioSource InstructionPlayer { get; }

        /// <summary>
        /// Synchronously returns the deserialized training course from given path.
        /// </summary>
        ICourse LoadCourse(string path);
    }
}
