using Innoactive.Creator.Core.Behaviors;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Behaviors
{
    public class BehaviorSequenceMenuItem : StepInspectorMenu.Item<IBehavior>
    {
        public override GUIContent DisplayedName
        {
            get
            {
                return new GUIContent("Behaviors Sequence");
            }
        }

        public override IBehavior GetNewItem()
        {
            return new BehaviorSequence();
        }
    }
}
