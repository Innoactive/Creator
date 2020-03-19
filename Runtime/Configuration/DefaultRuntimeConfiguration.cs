using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Innoactive.Creator.Core.Configuration.Modes;
using Innoactive.Creator.Core.Exceptions;
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
        private int currentModeIndex = 0;
        private string selectedCourseStreamingAssetsPath;
        private static ILogger entityStateLogger = Debug.unityLogger;

        private EntityStateLoggerConfig entityStateLoggerConfig = new EntityStateLoggerConfig
        {
            LogSteps = true,
            LogChapters = true,
            LogBehaviors = false,
            LogConditions = false,
            LogTransitions = false
        };

        /// <summary>
        /// The index of the current training mode.
        /// </summary>
        /// <exception cref="MissingModeException">Thrown when trying to access the current training mode index while there are no training modes available.</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown when the current training mode index is out of range when being accessed.</exception>
        protected virtual int CurrentModeIndex
        {
            get
            {
                int modeCount = AvailableModes.Count;

                if (modeCount == 0)
                {
                    throw new MissingModeException("You cannot access the current training mode index because there are no training modes available.");
                }

                if (currentModeIndex < modeCount)
                {
                    return currentModeIndex;
                }

                string message = string.Format("The current training mode index is set to {0} but the current number of available training modes is {1}.", currentModeIndex, modeCount);
                throw new IndexOutOfRangeException(message);
            }
            set
            {
                int modeCount = AvailableModes.Count;

                if (value >= 0 && value < modeCount)
                {
                    currentModeIndex = value;
                }
                else
                {
                    currentModeIndex = Mathf.Clamp(value, 0, modeCount);
                    Debug.LogWarningFormat("You tried to set the current training mode index to {0}. Use a valid non-negative index counting from 0. There are currently {1} modes available. The index was clamped to {2}.", value, modeCount, currentModeIndex);
                }
            }
        }

        private readonly IMode defaultMode = new Mode("Default", new WhitelistTypeRule<IOptional>());

        protected virtual IMode DefaultMode
        {
            get
            {
                return defaultMode;
            }
        }

        /// <inheritdoc />
        public event EventHandler<ModeChangedEventArgs> ModeChanged;

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


        /// <inheritdoc />
        public ICourseSerializer Serializer { get; set; } = new NewtonsoftJsonCourseSerializer();

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
        public virtual ILogger EntityStateLogger
        {
            get
            {
                return entityStateLogger;
            }
            set
            {
                entityStateLogger = value;
            }
        }

        /// <inheritdoc />
        public virtual EntityStateLoggerConfig EntityStateLoggerConfig
        {
            get
            {
                return entityStateLoggerConfig;
            }
            set
            {
                entityStateLoggerConfig = value;
            }
        }

        /// <inheritdoc />
        public virtual ReadOnlyCollection<IMode> AvailableModes
        {
            get
            {
                return new ReadOnlyCollection<IMode>(new List<IMode> { DefaultMode });
            }
        }

        /// <inheritdoc />
        public virtual ICourse LoadCourse()
        {
            if (string.IsNullOrEmpty(RuntimeConfigurator.GetSelectedTrainingCourse()))
            {
                throw new ArgumentException("A training course is not selected in the [TRAINING_CONFIGURATION] game object.");
            }

            string selectedTrainingCourse = RuntimeConfigurator.GetSelectedTrainingCourse();
            byte[] serialized = FileManager.Read(selectedTrainingCourse);

            return Serializer.CourseFromByteArray(serialized);
        }

        /// <inheritdoc />
        public virtual void SetMode(int index)
        {
            CurrentModeIndex = index;

            if (ModeChanged != null)
            {
                ModeChanged(this, new ModeChangedEventArgs(GetCurrentMode()));
            }
        }

        /// <inheritdoc />
        public virtual IMode GetCurrentMode()
        {
            return AvailableModes[CurrentModeIndex];
        }

        /// <inheritdoc />
        public virtual int GetCurrentModeIndex()
        {
            return CurrentModeIndex;
        }
    }
}
