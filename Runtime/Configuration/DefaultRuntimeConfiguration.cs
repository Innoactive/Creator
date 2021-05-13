using UnityEngine;
using System;
using System.Collections.Generic;
using VPG.Creator.Core.Configuration.Modes;
using VPG.Creator.Core.IO;
using VPG.Creator.Core.SceneObjects;
using VPG.Creator.Core.Properties;
using VPG.Creator.Core.Serialization;
using VPG.Creator.Core.Serialization.NewtonsoftJson;

namespace VPG.Creator.Core.Configuration
{
    /// <summary>
    /// Training runtime configuration which is used if no other was implemented.
    /// </summary>
    public class DefaultRuntimeConfiguration : BaseRuntimeConfiguration
    {
        private AudioSource instructionPlayer;

        /// <summary>
        /// Default mode which white lists everything.
        /// </summary>
        public static readonly IMode DefaultMode = new Mode("Default", new WhitelistTypeRule<IOptional>());

        public DefaultRuntimeConfiguration()
        {
            Modes = new BaseModeHandler(new List<IMode> {DefaultMode});
        }

        /// <inheritdoc />
        public override TrainingSceneObject Trainee
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
        public override AudioSource InstructionPlayer
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

    }
}
