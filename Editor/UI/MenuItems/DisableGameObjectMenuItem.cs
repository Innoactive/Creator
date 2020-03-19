using Innoactive.Creator.Core.Behaviors;
using Innoactive.CreatorEditor;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Behaviors
{
    public class DisableGameObjectMenuItem : StepInspectorMenu.Item<IBehavior>
    {
        public override GUIContent DisplayedName
        {
            get
            {
                return new GUIContent("Disable Object");
            }
        }

        public override IBehavior GetNewItem()
        {
            return new DisableGameObjectBehavior();
        }
    }
}
