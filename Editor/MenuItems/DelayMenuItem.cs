using Innoactive.Hub.Training.Editors.Configuration;
using UnityEngine;

namespace Innoactive.Hub.Training.Behaviors.Editors
{
    public class DelayMenuItem : Menu.Item<IBehavior>
    {
        public override GUIContent DisplayedName
        {
            get { return new GUIContent("Delay"); }
        }

        public override IBehavior GetNewItem()
        {
            return new DelayBehavior();
        }
    }
}
