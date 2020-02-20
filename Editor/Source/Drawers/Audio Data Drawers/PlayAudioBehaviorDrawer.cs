using System;
using UnityEngine;
using Innoactive.Hub.Training.Audio;
using Innoactive.Hub.Training.Behaviors;

namespace Innoactive.Hub.Training.Editors.Drawers
{
    /// <summary>
    /// Default drawer for <see cref="PlayAudioBehavior"/>. It sets displayed name to "Play Audio File".
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

            if (behavior.AudioData is ResourceAudio)
            {
                return new GUIContent("Play Audio File");
            }

            return base.GetTypeNameLabel(value, declaredType);
        }
    }
}
