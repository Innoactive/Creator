using Innoactive.Hub.Training.Editors.Configuration;
using UnityEngine;

namespace Innoactive.Hub.Training.Conditions.Editors
{
    public class ReleasedMenuItem : Menu.Item<ICondition>
    {
        public override GUIContent DisplayedName
        {
            get
            {
                return new GUIContent("Release Object");
            }
        }

        public override ICondition GetNewItem()
        {
            return new ReleasedCondition();
        }
    }
}
