using Innoactive.Creator.Core.Behaviors;
using Innoactive.CreatorEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Behaviors
{
    public class DelayMenuItem : StepInspectorMenu.Item<IBehavior>
    {
        public override GUIContent DisplayedName
        {
            get { return new GUIContent("Delay"); }
        }

        public override IBehavior GetNewItem()
        {
            return new DelayBehavior();
        }
    }
}
