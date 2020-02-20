using UnityEngine;
using Innoactive.Creator.Internationalization;
using Innoactive.Hub.Training.Editors.Configuration;

namespace Innoactive.Hub.Training.Behaviors.Editors
{
    public class PlayResourceAudioMenuItem : Menu.Item<IBehavior>
    {
        public override GUIContent DisplayedName
        {
            get
            {
                return new GUIContent("Audio/Play Audio File");
            }
        }

        public override IBehavior GetNewItem()
        {
            return new PlayAudioBehavior(new Audio.ResourceAudio(new LocalizedString()), BehaviorExecutionStages.Activation, true);
        }
    }
}
