using Innoactive.Hub.Training.Editors.Configuration;
using UnityEngine;

namespace Innoactive.Hub.Training.Behaviors.Editors
{
    public class MoveObjectMenuItem : Menu.Item<IBehavior>
    {
        public override GUIContent DisplayedName
        {
            get
            {
                return new GUIContent("Move Object");
            }
        }

        public override IBehavior GetNewItem()
        {
            return new MoveObjectBehavior();
        }
    }
}
