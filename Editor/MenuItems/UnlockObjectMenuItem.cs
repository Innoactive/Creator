using Innoactive.Hub.Training.Editors.Configuration;
using UnityEngine;

namespace Innoactive.Hub.Training.Behaviors.Editors
{
    public class UnlockObjectMenuItem : Menu.Item<IBehavior>
    {
        public override GUIContent DisplayedName
        {
            get
            {
                return new GUIContent("Unlock Object");
            }
        }

        public override IBehavior GetNewItem()
        {
            return new UnlockObjectBehavior();
        }
    }
}
