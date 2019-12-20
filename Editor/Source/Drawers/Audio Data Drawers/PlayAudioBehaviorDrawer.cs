using System;
using Innoactive.Hub.Training.Audio;
using Innoactive.Hub.Training.Behaviors;
using UnityEngine;

namespace Innoactive.Hub.Training.Editors.Drawers
{
    /// <summary>
    /// Default drawer for <see cref="PlayAudioBehavior"/>. It changes displayed name to "Play TTS Audio" or "Play Audio File", depending on which AudioData is used.
    /// </summary>
    [DefaultTrainingDrawer(typeof(PlayAudioBehavior.EntityData))]
    public class PlayAudioBehaviorDrawer : NameableDrawer
    {
        /// <inheritdoc />
        protected override GUIContent GetTypeNameLabel(object value, Type declaredType)
        {
            PlayAudioBehavior.EntityData behavior = value as PlayAudioBehavior.EntityData;

            if (behavior == null)
            {
                return base.GetTypeNameLabel(value, declaredType);
            }

            if (behavior.AudioData is TextToSpeechAudio)
            {
                return new GUIContent("Play TTS Audio");
            }

            if (behavior.AudioData is ResourceAudio)
            {
                return new GUIContent("Play Audio File");
            }

            return base.GetTypeNameLabel(value, declaredType);
        }
    }
}
