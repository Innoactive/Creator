using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Innoactive.Hub.Config;
using Innoactive.Hub.TextToSpeech;
using Innoactive.Hub.Training.Configuration.Modes;
using Innoactive.Hub.Training.Exceptions;
using Innoactive.Hub.Training.SceneObjects;
using Innoactive.Hub.Training.Utils.Serialization;
using UnityEngine;

namespace Innoactive.Hub.Training.Configuration
{
    /// <summary>
    /// Training runtime configuration which is used if no other was implemented.
    /// </summary>
    public class DefaultRuntimeConfiguration : IRuntimeConfiguration
    {
        private AudioSource instructionPlayer;
        private TextToSpeechConfig textToSpeechConfig;
        private ISceneObjectRegistry sceneObjectRegistry;
        private int currentModeIndex = 0;
        private string selectedCourseStreamingAssetsPath;
        private static ILogger entityStateLogger = Debug.unityLogger;

        private EntityStateLoggerConfig entityStateLoggerConfig = new EntityStateLoggerConfig()
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
        /// <exception cref="Innoactive.Hub.Training.Exceptions.MissingModeException">Thrown when trying to access the current training mode index while there are no training modes available.</exception>
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
        public virtual TrainingSceneObject Trainee
        {
            get
            {
                TrainingSceneObject headset = VRTK.VRTK_DeviceFinder.HeadsetTransform().GetComponent<TrainingSceneObject>();

                if (headset == null)
                {
                    headset = VRTK.VRTK_DeviceFinder.HeadsetTransform().gameObject.AddComponent<TrainingSceneObject>();
                    headset.ChangeUniqueName("Trainee");
                }

                return headset;
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
        public virtual TextToSpeechConfig TextToSpeechConfig
        {
            get
            {
                return textToSpeechConfig;
            }
            set
            {
                textToSpeechConfig = value;
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

            string pathToFile = Path.Combine(Application.streamingAssetsPath, RuntimeConfigurator.GetSelectedTrainingCourse()).Replace('/', Path.DirectorySeparatorChar);

            if (File.Exists(pathToFile) == false)
            {
                throw new ArgumentException("The file or the path to the file could not be found.", pathToFile);
            }

            string serialized = File.ReadAllText(pathToFile);
            return JsonTrainingSerializer.Deserialize(serialized);
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

        public DefaultRuntimeConfiguration()
        {
            TextToSpeechConfig config = new TextToSpeechConfig();
            if (config.Exists())
            {
                textToSpeechConfig = config.Load();
            }
        }
    }
}
