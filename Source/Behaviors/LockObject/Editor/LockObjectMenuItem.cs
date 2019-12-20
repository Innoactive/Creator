using Innoactive.Hub.Training.Editors.Configuration;
using UnityEngine;

namespace Innoactive.Hub.Training.Behaviors.Editors
{
    public class LockObjectMenuItem : Menu.Item<IBehavior>
    {
        public override GUIContent DisplayedName
        {
            get
            {
                return new GUIContent("Lock Object");
            }
        }

        public override IBehavior GetNewItem()
        {
            return new LockObjectBehavior();
        }
    }
}
