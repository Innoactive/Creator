using Innoactive.Hub.Training.Editors.Configuration;
using UnityEngine;

namespace Innoactive.Hub.Training.Behaviors.Editors
{
    public class EnableGameObjectMenuItem : Menu.Item<IBehavior>
    {
        public override GUIContent DisplayedName
        {
            get
            {
                return new GUIContent("Enable Object");
            }
        }

        public override IBehavior GetNewItem()
        {
            return new EnableGameObjectBehavior();
        }
    }
}
