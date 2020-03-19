using Innoactive.Creator.Core.Conditions;
using Innoactive.CreatorEditor.Configuration;
using UnityEngine;

namespace Innoactive.CreatorEditor.UI.Conditions
{
    public class ObjectInColliderMenuItem : StepInspectorMenu.Item<ICondition>
    {
        public override GUIContent DisplayedName
        {
            get
            {
                return new GUIContent("Move Object into Collider");
            }
        }

        public override ICondition GetNewItem()
        {
            return new ObjectInColliderCondition();
        }
    }
}
