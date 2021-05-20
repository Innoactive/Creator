using System.Runtime.Serialization;
using VPG.Core.Attributes;
using VPG.Core.Internationalization;
using UnityEngine;

namespace VPG.Core.Audio
{
    /// <summary>
    /// Unity resource based audio data.
    /// </summary>
    [DataContract(IsReference = true)]
    [DisplayName("Play Audio File")]
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

            if (path == null || string.IsNullOrEmpty(path.Value))
            {
                Debug.LogWarningFormat("Path to audio file is not defined.");
                return;
            }

            AudioClip = Resources.Load<AudioClip>(path.Value);

            if (HasAudioClip == false)
            {
                Debug.LogWarningFormat("Given path '{0}' to resource has returned no audio clip", path);
            }
        }

        /// <inheritdoc/>
        public bool IsEmpty()
        {
            return path == null || (string.IsNullOrEmpty(path.Key) && string.IsNullOrEmpty(path.DefaultText));
        }
    }
}
