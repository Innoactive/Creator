using Innoactive.Hub.Training.Editors.Configuration;
using UnityEngine;

namespace Innoactive.Hub.Training.Conditions.Editors
{
    public class SnappedMenuItem : Menu.Item<ICondition>
    {
        public override GUIContent DisplayedName
        {
            get
            {
                return new GUIContent("Snap Object");
            }
        }

        public override ICondition GetNewItem()
        {
            return new SnappedCondition();
        }
    }
}
