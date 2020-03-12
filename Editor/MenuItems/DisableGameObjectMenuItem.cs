using Innoactive.Hub.Training.Editors.Configuration;
using UnityEngine;

namespace Innoactive.Hub.Training.Behaviors.Editors
{
    public class DisableGameObjectMenuItem : Menu.Item<IBehavior>
    {
        public override GUIContent DisplayedName
        {
            get
            {
                return new GUIContent("Disable Object");
            }
        }

        public override IBehavior GetNewItem()
        {
            return new DisableGameObjectBehavior();
        }
    }
}
