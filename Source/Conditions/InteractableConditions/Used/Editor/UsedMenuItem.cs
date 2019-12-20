using Innoactive.Hub.Training.Editors.Configuration;
using UnityEngine;

namespace Innoactive.Hub.Training.Conditions.Editors
{
    public class UsedMenuItem : Menu.Item<ICondition>
    {
        public override GUIContent DisplayedName
        {
            get { return new GUIContent("Use Object"); }
        }

        public override ICondition GetNewItem()
        {
            return new UsedCondition();
        }
    }
}
