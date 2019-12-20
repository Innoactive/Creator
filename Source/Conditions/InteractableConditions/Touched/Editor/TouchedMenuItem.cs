using Innoactive.Hub.Training.Editors.Configuration;
using UnityEngine;

namespace Innoactive.Hub.Training.Conditions.Editors
{
    public class TouchedMenuItem : Menu.Item<ICondition>
    {
        public override GUIContent DisplayedName
        {
            get
            {
                return new GUIContent("Touch Object");
            }
        }

        public override ICondition GetNewItem()
        {
            return new TouchedCondition();
        }
    }
}
