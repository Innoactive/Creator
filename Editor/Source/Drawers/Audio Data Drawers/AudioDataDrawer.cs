using System;
using System.Reflection;
using UnityEngine;
using Innoactive.Hub.Training.Audio;
using Innoactive.Creator.Localization;

namespace Innoactive.Hub.Training.Editors.Drawers
{
    /// <summary>
    /// Training drawer for <see cref="IAudioData"/> members.
    /// </summary>
    [DefaultTrainingDrawer(typeof(IAudioData))]
    public class AudioDataDrawer : ObjectDrawer
    {
        /// <inheritdoc />
        public override Rect Draw(Rect rect, object currentValue, Action<object> changeValueCallback, GUIContent label)
        {
            ResourceAudio resourceAudio = currentValue as ResourceAudio;

            if (resourceAudio != null)
            {
                if (resourceAudio.Path == null)
                {
                    resourceAudio.Path = new LocalizedString();
                    changeValueCallback(resourceAudio);
                }

                ITrainingDrawer pathDrawer = DrawerLocator.GetDrawerForMember(resourceAudio.GetType().GetProperty("Path", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance), typeof(LocalizedString));
                return pathDrawer.Draw(rect, resourceAudio.Path, newPath =>
                {
                    resourceAudio.Path = (LocalizedString) newPath;
                    changeValueCallback(resourceAudio);
                }, label);
            }

            return base.Draw(rect, currentValue, changeValueCallback, label);
        }
    }
}
