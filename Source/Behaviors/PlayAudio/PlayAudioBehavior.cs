using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using Innoactive.Hub.Training.Audio;
using Innoactive.Hub.Training.Attributes;
using Innoactive.Hub.Training.Configuration;
using Innoactive.Hub.Training.Configuration.Modes;

namespace Innoactive.Hub.Training.Behaviors
{
    [DataContract(IsReference = true)]
    public class PlayAudioBehavior : Behavior<PlayAudioBehavior.EntityData>, IOptional
    {
        [DataContract(IsReference = true)]
        public class EntityData : IBackgroundBehaviorData
        {
            /// <summary>
            /// An audio data that contains an audio clip to play.
            /// </summary>
            [DataMember]
            public IAudioData AudioData { get; set; }

            /// <summary>
            /// A property that determines if the audio should be played at activation or deactivation (or both).
            /// </summary>
            [DataMember]
            [DisplayName("Execution stages")]
            public BehaviorExecutionStages ExecutionStages { get; set; }

            public AudioSource AudioPlayer { get; set; }

            public Metadata Metadata { get; set; }
            public string Name { get; set; }

            /// <inheritdoc />
            public bool IsBlocking { get; set; }
        }

        private class PlayAudioProcess : IStageProcess<EntityData>
        {
            private readonly BehaviorExecutionStages executionStages;

            public PlayAudioProcess(BehaviorExecutionStages executionStages)
            {
                this.executionStages = executionStages;
            }

            public void Start(EntityData data)
            {
                if (data.AudioPlayer == null)
                {
                    data.AudioPlayer = RuntimeConfigurator.Configuration.InstructionPlayer;
                }

                if ((data.ExecutionStages & executionStages) > 0)
                {
                    if (data.AudioData.HasAudioClip)
                    {
                        data.AudioPlayer.clip = data.AudioData.AudioClip;
                        data.AudioPlayer.Play();
                    }
                    else
                    {
                        Debug.LogWarning("AudioData has no audio clip.");
                    }
                }
            }

            public IEnumerator Update(EntityData data)
            {
                while ((data.ExecutionStages & executionStages) > 0 && data.AudioPlayer.isPlaying)
                {
                    yield return null;
                }
            }

            public void End(EntityData data)
            {
                if ((data.ExecutionStages & executionStages) > 0)
                {
                    data.AudioPlayer.clip = null;
                }
            }

            public void FastForward(EntityData data)
            {
                if ((data.ExecutionStages & executionStages) > 0 && data.AudioPlayer.isPlaying)
                {
                    data.AudioPlayer.Stop();
                }
            }
        }

        protected PlayAudioBehavior() : this(null, BehaviorExecutionStages.None)
        {
        }

        public PlayAudioBehavior(IAudioData audioData, BehaviorExecutionStages executionStages, AudioSource audioPlayer = null, string name = "Play Audio")
        {
            Data = new EntityData
            {
                AudioData = audioData,
                ExecutionStages = executionStages,
                AudioPlayer = audioPlayer,
                Name = name,
                IsBlocking = true
            };
        }

        public PlayAudioBehavior(IAudioData audioData, BehaviorExecutionStages executionStages, bool isBlocking, AudioSource audioPlayer = null, string name = "Play Audio") : this(audioData, executionStages, audioPlayer, name)
        {
            Data.IsBlocking = isBlocking;
        }

        private readonly IProcess<EntityData> process = new Process<EntityData>(new PlayAudioProcess(BehaviorExecutionStages.Activation), new EmptyStageProcess<EntityData>(), new PlayAudioProcess(BehaviorExecutionStages.Deactivation));

        protected override IProcess<EntityData> Process
        {
            get
            {
                return process;
            }
        }
    }
}
