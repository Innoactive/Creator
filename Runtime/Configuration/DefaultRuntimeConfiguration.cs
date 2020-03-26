using UnityEngine;
using System;
using System.Collections.Generic;
using Innoactive.Creator.Core.Configuration.Modes;
using Innoactive.Creator.Core.IO;
using Innoactive.Creator.Core.SceneObjects;
using Innoactive.Creator.Core.Properties;
using Innoactive.Creator.Core.Serialization;
using Innoactive.Creator.Core.Serialization.NewtonsoftJson;

namespace Innoactive.Creator.Core.Configuration
{
    /// <summary>
    /// Training runtime configuration which is used if no other was implemented.
    /// </summary>
    public class DefaultRuntimeConfiguration : IRuntimeConfiguration
    {
        private AudioSource instructionPlayer;
        private ISceneObjectRegistry sceneObjectRegistry;
        private string selectedCourseStreamingAssetsPath;

        /// <summary>
        /// Default mode which white lists everything.
        /// </summary>
        public static readonly IMode DefaultMode = new Mode("DefaultMode", new WhitelistTypeRule<IOptional>());

        /// <inheritdoc />
        public ISceneObjectRegistry SceneObjectRegistry
        {
            get
            {
                if (sceneObjectRegistry == null)
                {
                    sceneObjectRegistry = new SceneObjectRegistry();
                }

                return sceneObjectRegistry;
            }
        }

        protected DefaultRuntimeConfiguration()
        {
            List<IMode> modeList = new List<IMode>();
            modeList.Add(DefaultMode);
            Modes = new BaseModeHandler(modeList);
        }

        /// <inheritdoc />
        public ICourseSerializer Serializer { get; set; } = new NewtonsoftJsonCourseSerializer();

        /// <inheritdoc />
        public IModeHandler Modes { get; protected set; }

        /// <inheritdoc />
        public virtual TrainingSceneObject Trainee
        {
            get
            {
                TrainingSceneObject trainee = GameObject.FindObjectOfType<TraineeSceneObject>();

                if (trainee == null)
                {
                    throw new Exception("Could not find a TraineeSceneObject in the scene.");
                }

                return trainee;
            }
        }

        /// <inheritdoc />
        public virtual AudioSource InstructionPlayer
        {
            get
            {
                if (instructionPlayer == null || instructionPlayer.Equals(null))
                {
                    instructionPlayer = Trainee.gameObject.AddComponent<AudioSource>();
                }

                return instructionPlayer;
            }
        }

        /// <inheritdoc />
        public virtual ICourse LoadCourse(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Given path is null or empty!");
            }
            byte[] serialized = FileManager.Read(path);
            return Serializer.CourseFromByteArray(serialized);
        }
    }
}
