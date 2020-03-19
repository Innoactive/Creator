using System;
using Innoactive.Creator.Core.Audio;
using Innoactive.Creator.Core.Behaviors;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Drawers
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
