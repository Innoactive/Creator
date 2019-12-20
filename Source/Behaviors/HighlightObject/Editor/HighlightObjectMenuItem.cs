using Innoactive.Hub.Training.Editors.Configuration;
using UnityEngine;

namespace Innoactive.Hub.Training.Behaviors.Editors
{
    public class HighlightObjectMenuItem : Menu.Item<IBehavior>
    {
        public override GUIContent DisplayedName
        {
            get { return new GUIContent("Highlight Object"); }
        }

        public override IBehavior GetNewItem()
        {
            return new HighlightObjectBehavior();
        }
    }
}
