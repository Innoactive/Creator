using Innoactive.Creator.Core.Behaviors;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Behaviors
{
    public class HighlightObjectMenuItem : StepInspectorMenu.Item<IBehavior>
    {
        public override GUIContent DisplayedName
        {
            get { return new GUIContent("Highlight Object"); }
        }

        public override IBehavior GetNewItem()
        {
            return new HighlightObjectBehavior();
        }
    }
}
