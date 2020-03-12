using Innoactive.Hub.Training.Editors.Configuration;
using UnityEngine;

namespace Innoactive.Hub.Training.Conditions.Editors
{
    public class Timeout : Menu.Item<ICondition>
    {
        public override GUIContent DisplayedName
        {
            get { return new GUIContent("Timeout"); }
        }

        public override ICondition GetNewItem()
        {
            return new TimeoutCondition();
        }
    }
}
