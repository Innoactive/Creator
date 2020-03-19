using Innoactive.Creator.Core.Conditions;
using Innoactive.CreatorEditor.Configuration;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Conditions
{
    public class ObjectInRangeMenuItem : StepInspectorMenu.Item<ICondition>
    {
        public override GUIContent DisplayedName
        {
            get
            {
                return new GUIContent("Object Nearby");
            }
        }

        public override ICondition GetNewItem()
        {
            return new ObjectInRangeCondition();
        }
    }
}
