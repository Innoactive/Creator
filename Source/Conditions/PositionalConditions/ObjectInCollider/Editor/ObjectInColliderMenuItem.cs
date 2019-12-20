using Innoactive.Hub.Training.Editors.Configuration;
using UnityEngine;

namespace Innoactive.Hub.Training.Conditions.Editors
{
    public class ObjectInColliderMenuItem : Menu.Item<ICondition>
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
