using Innoactive.Creator.Core.Behaviors;
using Innoactive.CreatorEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Behaviors
{
    public class LockObjectMenuItem : StepInspectorMenu.Item<IBehavior>
    {
        public override GUIContent DisplayedName
        {
            get
            {
                return new GUIContent("Lock Object");
            }
        }

        public override IBehavior GetNewItem()
        {
            return new LockObjectBehavior();
        }
    }
}
