using Innoactive.Creator.Core.Behaviors;
using Innoactive.CreatorEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Behaviors
{
    public class UnlockObjectMenuItem : StepInspectorMenu.Item<IBehavior>
    {
        public override GUIContent DisplayedName
        {
            get
            {
                return new GUIContent("Unlock Object");
            }
        }

        public override IBehavior GetNewItem()
        {
            return new UnlockObjectBehavior();
        }
    }
}
