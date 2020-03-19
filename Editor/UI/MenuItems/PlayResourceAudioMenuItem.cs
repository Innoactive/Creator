using Innoactive.Creator.Core.Audio;
using Innoactive.Creator.Core.Behaviors;
using Innoactive.Creator.Core.Internationalization;
using Innoactive.CreatorEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Behaviors
{
    public class PlayResourceAudioMenuItem : StepInspectorMenu.Item<IBehavior>
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
            return new PlayAudioBehavior(new ResourceAudio(new LocalizedString()), BehaviorExecutionStages.Activation, true);
        }
    }
}
