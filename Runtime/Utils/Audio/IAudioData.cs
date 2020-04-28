using System.Runtime.Serialization;
using UnityEngine;

namespace Innoactive.Creator.Core.Audio
{
    /// <summary>
    /// This class provides audio data in form of an AudioClip. Which also might not be loaded at the time needed,
    /// check first if there can be one provided.
    /// </summary>
    public interface IAudioData
    {
        /// <summary>
        /// Determs if the AudioSource has an AudioClip which can be played.
        /// </summary>
        bool HasAudioClip { get; }

        /// <summary>
        /// The AudioClip of this source, can be null. Best check first with HasAudio.
        /// </summary>
        AudioClip AudioClip { get; }
    }
}
