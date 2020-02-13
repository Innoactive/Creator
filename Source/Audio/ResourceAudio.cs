using System.Runtime.Serialization;
using Innoactive.Hub.Training.Attributes;
using Newtonsoft.Json;
using UnityEngine;

namespace Innoactive.Hub.Training.Audio
{
    /// <summary>
    /// Unity resource based audio data.
    /// </summary>
    [DataContract(IsReference = true)]
    public class ResourceAudio : IAudioData
    {
        private LocalizedString path;

        [DataMember]
        [UsesSpecificTrainingDrawer("ResourceAudioDataLocalizedStringDrawer")]
        public LocalizedString Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
                InitializeAudioClip();
            }
        }

        public ResourceAudio(LocalizedString path)
        {
            Path = path;
        }

        [JsonConstructor]
        protected ResourceAudio()
        {
            path = new LocalizedString();
        }

        public bool HasAudioClip
        {
            get
            {
                return AudioClip != null;
            }
        }

        public AudioClip AudioClip { get; private set; }

        private void InitializeAudioClip()
        {
            if (Application.isPlaying == false)
            {
                return;
            }

            if (path?.Value == null)
            {
                Debug.LogWarningFormat("Path to audiofile is not defined.");
                return;
            }

            AudioClip = Resources.Load<AudioClip>(path.Value);

            if (HasAudioClip == false)
            {
                Debug.LogWarningFormat("Given path '{0}' to resource has returned no audio clip", path);
            }
        }
    }
}
