using System;
using UnityEngine;
using VPG.Creator.Core.Configuration.Modes;
using VPG.Creator.Core.SceneObjects;
using VPG.Creator.Core.Serialization;

namespace VPG.Creator.Core.Configuration
{
    /// <summary>
    /// An interface for training runtime configurations. Implement it to create your own.
    /// </summary>
    [Obsolete("To be more flexible with development we switched to an abstract class as configuration base, consider using BaseRuntimeConfiguration.")]
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
