using Innoactive.Creator.Core.Behaviors;
using Innoactive.CreatorEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Behaviors
{
    public class MoveObjectMenuItem : StepInspectorMenu.Item<IBehavior>
    {
        public override GUIContent DisplayedName
        {
            get
            {
                return new GUIContent("Move Object");
            }
        }

        public override IBehavior GetNewItem()
        {
            return new MoveObjectBehavior();
        }
    }
}
