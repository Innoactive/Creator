using Innoactive.Hub.Training.Editors.Configuration;
using UnityEngine;

namespace Innoactive.Hub.Training.Conditions.Editors
{
    public class ObjectInRangeMenuItem : Menu.Item<ICondition>
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
